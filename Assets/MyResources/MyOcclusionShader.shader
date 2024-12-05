Shader "Custom/TransparentBlockingMeshesAndSkybox"
{
    Properties
    {
        _Color ("Tint Color", Color) = (1,1,1,0.5) // Tint color with adjustable alpha
    }
    SubShader
    {
        // This sets the rendering order just after opaque objects to ensure it blocks objects behind it
        Tags { "Queue" = "Geometry+1" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            // The object should be invisible in terms of color
            ColorMask 0

            // Write to the depth buffer to block objects behind it
            ZWrite On

            // Allow the skybox to be visible behind the object
            ZTest LEqual
            ZClip Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag() : SV_Target
            {
                // Discard the fragment, making the object transparent
                discard;
                return float4(0,0,0,0);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
