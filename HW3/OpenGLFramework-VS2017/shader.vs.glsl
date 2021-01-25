#version 330

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec3 aNormal;
layout (location = 3) in vec2 aTexCoord;

out vec2 texCoord;
out vec3 vertex_normal;
out vec3 vertex_pos;

uniform mat4 um4p;	// projection matrix
uniform mat4 um4v;	// camera viewing transformation matrix
uniform mat4 um4m;	// rotation matrix

void main() 
{
	texCoord = aTexCoord;
	gl_Position = um4p * um4v * um4m * vec4(aPos, 1.0);

	vec4 normal = transpose(inverse(um4v * um4m)) * vec4(aNormal.x, aNormal.y, aNormal.z, 0.0);
	vertex_normal = vec3(normal.x, normal.y, normal.z);

	vec4 pos = (um4v * um4m) * vec4(aPos.x, aPos.y, aPos.z, 1.0);
	vertex_pos = vec3(pos.x, pos.y, pos.z);
}
