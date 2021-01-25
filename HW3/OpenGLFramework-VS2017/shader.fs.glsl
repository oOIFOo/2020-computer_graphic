#version 330

in vec2 texCoord;
in vec3 vertex_normal;
in vec3 vertex_pos;

out vec4 fragColor;

uniform vec3 ka;
uniform vec3 kd;
uniform vec3 ks;

// [TODO] passing texture from main.cpp
// Hint: sampler2D
uniform sampler2D TexMap;

void main() {
	// [TODO] sampleing from texture
	// Hint: texture
//---------------------------ambient-----------------------------
	vec3 ambient_strength = vec3(0.15f, 0.15f, 0.15f);
	vec3 La = ambient_strength;
	
//---------------------------diffuse-----------------------------
	vec3 Ld = vec3(1.0f, 1.0f, 1.0f);
	vec3 norm = normalize(vertex_normal);
	vec3 lightDir = normalize(vec3(1.0f, 1.0f, 1.0f));
	float diff = max(dot(norm, lightDir), 0.0);

//---------------------------specular-----------------------------
	vec3 Ls = vec3(1.0f, 1.0f, 1.0f);
	vec3 reflectDir = normalize(vec3(1.0f, 1.0f, 1.0f) - vertex_pos);
	float spec = pow(max(dot(reflectDir, norm), 0.0), 64);

	vec3 result = La * ka + Ld * kd * diff  + Ls * ks * spec;

	fragColor = vec4(result, 1.0f) * texture2D(TexMap,texCoord.xy);
}
