#version 450

const float TILE_ATLAS_TILE_SIZE = 1.00 / 256.00;

layout (set = 1, binding = 1) uniform fTileLayerParameters { int fTilesWide; int fTilesHigh; int fLayerCount; int _unused; };
layout (set = 1, binding = 2) uniform utexture2DArray fTileLayerData;
layout (set = 1, binding = 3) uniform sampler fTileLayerDataSampler;

layout (set = 2, binding = 4) uniform texture2D fTileAtlas;
layout (set = 2, binding = 5) uniform sampler fTileAtlasSampler;

layout (location = 0) in vec2 fTexCoords; // {x=[0.00, 1.00], y=[0.00, 1.00]}
layout (location = 1) in vec2 fTileCoords; // {x=[0.00, <number of tiles wide>], y=[0.00, <number of tiles high>]}

layout (location = 0) out vec4 oColor;

#define TILE_INDEX(data) data.x
#define TILE_INDEX_TO_UVEC2(index) uvec2(index & 0x00ffu, (index & 0xff00u) >> 8u)
#define TILE_INDEX_TO_VEC2(index) vec2(float(index & 0x00ffu), float((index & 0xff00u) >> 8u))

void main()
{
  ivec2 tileCoords = ivec2(fTileCoords); // {x=[0, <number of tiles wide>], y=[0, <number of tiles high>]}
  vec2 localTileCoords = mod(fTileCoords, 1.00); // {x=[0.00, 1.00], y=[0.00, 1.00]}

  oColor = vec4(0.00, 0.00, 0.00, 0.00);

  for(int i = 0; i < fLayerCount; i++)
  {
    uvec4 data = texelFetch(usampler2DArray(fTileLayerData, fTileLayerDataSampler), ivec3(tileCoords, i), 0);
    uint tileIndex = TILE_INDEX(data);
    vec4 color = texture(sampler2D(fTileAtlas, fTileAtlasSampler), (localTileCoords + TILE_INDEX_TO_VEC2(tileIndex)) * TILE_ATLAS_TILE_SIZE);
    oColor = mix(oColor, color, color.a);
  }
}
