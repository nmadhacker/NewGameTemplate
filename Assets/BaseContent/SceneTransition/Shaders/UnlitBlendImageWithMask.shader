Shader "Unlit/UnlitBlendImageWithMask" {
    Properties {
        _Cutoff ("Cutoff", Range (0,1)) = 0.5
        _CutoffRange ("Cutoff Range", Range (0,1)) = 0.1
        _ImgTex ("Base (RGB)", 2D) = "white" {}
        _MainTex ("Gradient Mask", 2D) = "white" {}        
    }
        
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        Lighting OFF
       
        CGPROGRAM
        #pragma surface surf Unlit alpha noambient novertexlights nodirlightmap nolightmap noforwardadd
 
        half4 LightingUnlit (SurfaceOutput s, half3 dir, half atten) {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }
 
        sampler2D _ImgTex;
        sampler2D _MainTex;
        half _Cutoff;
        half _CutoffRange;
        float _InvertMask;
 
        struct Input {
            float2 uv_ImgTex;
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            
            half4 c = tex2D (_ImgTex, IN.uv_ImgTex);
            half d = tex2D (_MainTex, IN.uv_MainTex).r;

            o.Albedo = c.rgb;
 
            half r = _Cutoff * (1 + _CutoffRange * 2) - _CutoffRange;
            o.Alpha = (d - r) * (1 / (_CutoffRange));
        }
        ENDCG
    }
    Fallback "Diffuse"
}