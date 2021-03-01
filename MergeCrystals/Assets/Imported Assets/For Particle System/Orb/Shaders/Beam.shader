// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ErbGameArt/Beam" {
    Properties {
        _MainTexture ("MainTexture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _EmissivePower ("EmissivePower", Float ) = 5
        _OpacityPower ("OpacityPower", Float ) = 1.5
        _Noise ("Noise", 2D) = "white" {}
        _U_Speed ("U_Speed", Float ) = 0
        _V_Speed ("V_Speed", Float ) = 0.5
        _NoisePower ("NoisePower", Float ) = 1.5
        _FinalOpacity ("FinalOpacity", Float ) = 1
        _U_Speed_2 ("U_Speed_2", Float ) = 0.1
        _V_Speed_2 ("V_Speed_2", Float ) = -0.4
        _SecondaryNoise ("SecondaryNoise", 2D) = "white" {}
        _SecondaryNoisePower ("SecondaryNoisePower", Float ) = 1
        _DistortionPower ("DistortionPower", Float ) = 0.02
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float4 _Color;
            uniform float _EmissivePower;
            uniform float _OpacityPower;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _U_Speed;
            uniform float _V_Speed;
            uniform float _NoisePower;
            uniform float _FinalOpacity;
            uniform float _U_Speed_2;
            uniform float _V_Speed_2;
            uniform sampler2D _SecondaryNoise; uniform float4 _SecondaryNoise_ST;
            uniform float _SecondaryNoisePower;
            uniform float _DistortionPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float4 node_3262 = _Time + _TimeEditor;
                float2 node_1800 = ((float2(_U_Speed,_V_Speed)*node_3262.g)+i.uv0);
                float4 node_5766 = tex2D(_Noise,TRANSFORM_TEX(node_1800, _Noise));
                float3 node_2541 = pow(node_5766.rgb,_NoisePower);
                float4 node_6236 = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
                float3 node_7726 = (_EmissivePower*(node_6236.rgb*_Color.rgb*i.vertexColor.rgb*i.vertexColor.a));
                float3 node_5083 = (node_2541*node_7726);
                float3 node_5584 = (node_5083*_FinalOpacity);
                float2 node_309 = node_5584.rg;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_309*_DistortionPower);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 emissive = node_5584;
                float3 finalColor = emissive;
                float4 node_8329 = _Time + _TimeEditor;
                float2 node_6639 = ((float2(_U_Speed_2,_V_Speed_2)*node_8329.g)+i.uv0);
                float4 node_2432 = tex2D(_SecondaryNoise,TRANSFORM_TEX(node_6639, _SecondaryNoise));
                float3 node_9581 = ((_SecondaryNoisePower*node_2432.rgb)*node_2541);
                float node_5136 = ((node_6236.r*_Color.a*i.vertexColor.a)*_OpacityPower);
                float node_7204 = (node_9581.r*node_5136);
                return fixed4(lerp(sceneColor.rgb, finalColor,(_FinalOpacity*node_7204)),1);
            }
            ENDCG
        }
    }
    FallBack "Particles/Additive"
}
