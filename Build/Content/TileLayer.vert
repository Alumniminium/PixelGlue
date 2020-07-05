#version 450

layout (set = 0, binding = 0) uniform vTransformBuffer { mat4x4 vTransform; };

layout (location = 0) in vec2 vPosition;
layout (location = 1) in vec2 vTexCoords; // {x=[0.00, 1.00], y=[0.00, 1.00]}
layout (location = 2) in vec2 vTileCoords; // {x=[0.00, <number of tiles wide>], y=[0.00, <number of tiles high>]}

layout (location = 0) out vec2 fTexCoords;
layout (location = 1) out vec2 fTileCoords;

void main()
{
    gl_Position = vTransform * vec4(vPosition, 0.00, 1.00);
    fTexCoords = vTexCoords;
    fTileCoords = vTileCoords;
}
