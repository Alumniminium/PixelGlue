// MapRenderer.fx
//#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
//#else
//#define VS_SHADERMODEL vs_4_0     //_level_9_1
//#define PS_SHADERMODEL ps_4_0     //_level_9_1
//#endif

matrix World;
matrix View;
matrix Projection;

Texture2D TextureA; // primary texture.
sampler TextureSamplerA = sampler_state
{
    texture = <TextureA>;
    //magfilter = LINEAR; //minfilter = LINEAR; //mipfilter = LINEAR; //AddressU = mirror; //AddressV = mirror; 
};
//_______________________________________________________________
// techniques 
// Quad Draw  Position Color Texture
//_______________________________________________________________
struct VsInputQuad
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexureCoordinateA : TEXCOORD0;
};
struct VsOutputQuad
{
    float4 Position : SV_Position;
    float4 Color : COLOR0;
    float2 TexureCoordinateA : TEXCOORD0;
};
struct PsOutputQuad
{
    float4 Color : COLOR0;
};
// ____________________________
VsOutputQuad VertexShaderQuadDraw(VsInputQuad input)
{
    VsOutputQuad output;
    float4x4 vp = mul(View, Projection);
    float4x4 wvp = mul(World, vp);
    output.Position = mul(input.Position, wvp); // Transform by WorldViewProjection
    output.Color = input.Color;
    output.TexureCoordinateA = input.TexureCoordinateA;
    return output;
}
PsOutputQuad PixelShaderQuadDraw(VsOutputQuad input)
{
    PsOutputQuad output;
    output.Color = tex2D(TextureSamplerA, input.TexureCoordinateA) * input.Color;
    return output;
}

technique QuadDraw
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderQuadDraw();
        PixelShader = compile PS_SHADERMODEL PixelShaderQuadDraw();
    }
}