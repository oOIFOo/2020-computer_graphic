#version 330 core
# define M_PI 3.14159265358979323846

out vec4 FragColor;
in vec3 vertex_color;
in vec3 vertex_normal;
in vec3 vertex_pos; //viewspace

uniform vec3 ka;
uniform vec3 kd;
uniform vec3 ks;
uniform vec3 light_dir;
uniform vec3 light_pos; //viewspace
uniform vec3 intensity;
uniform int cur_light_mode;
uniform int shininess;
uniform int angle;
uniform int shade_mode;

void main() {
	vec3 result;
	//veiw_pos = (0, 0, 0) in view space 

//---------------------------ambient-----------------------------
	vec3 ambient_strength = vec3(0.15f, 0.15f, 0.15f);
	vec3 La = ambient_strength;
	
//---------------------------diffuse-----------------------------
	vec3 Ld = intensity;
	vec3 norm = normalize(vertex_normal);

	vec3 lightDir;
	if (cur_light_mode == 0)
		lightDir = normalize(light_pos);
	else if (cur_light_mode == 1 || cur_light_mode == 2)
		lightDir = normalize(light_pos - vertex_pos);

	float diff = max(dot(norm, lightDir), 0.0);

//---------------------------specular-----------------------------
	vec3 Ls = intensity;

	//half way vector
	vec3 reflectDir;
	if (cur_light_mode == 0) 
		reflectDir = normalize(light_pos - vertex_pos);
	else if (cur_light_mode == 1 || cur_light_mode == 2) 
		reflectDir = normalize((light_pos - vertex_pos) + vertex_pos);

	float spec = pow(max(dot(reflectDir, norm), 0.0), shininess);

//---------------------------attenuation-----------------------------
	float attenuation_d;
	float attenuation_s;
	if (cur_light_mode == 1){
		float distance = length(light_pos - vertex_pos);
		attenuation_d = 1.0 / (0.01 + 0.8 * distance + 0.1 * distance * distance);
	}
	else if (cur_light_mode == 2){
		float distance = length(light_pos - vertex_pos);
		attenuation_s = 1.0 / (0.05 + 0.3 * distance + 0.6 * distance * distance);
	}

//---------------------------result-----------------------------
	if (cur_light_mode == 0){
		//directional light mode
		result = La * ka + Ld * kd * diff  + Ls * ks * spec;
	}
	else if (cur_light_mode == 1){
		//point light mode
		result = attenuation_d * (La * ka + Ld * kd * diff +  Ls * ks * spec);
	}
	else if (cur_light_mode == 2){
		//spot light mode
		float cosine = dot(vertex_pos - light_pos, light_dir) / (length(vertex_pos - light_pos) * length(light_dir));
		if (cosine <= cos(angle * M_PI / 180)){
			result = La * ka;
		}
		else if (cosine > cos(angle * M_PI / 180)){
			float spot_effect = pow(max(dot(normalize(vertex_pos - light_pos), normalize(light_dir)), 0), 50);
			result = spot_effect * (La * ka + Ld * kd * diff + Ls * ks * spec);
		}
	}

	if (shade_mode == 0) FragColor = vec4(vertex_color, 1.0f);
	if (shade_mode == 1) FragColor = vec4(result, 1.0f);
}
