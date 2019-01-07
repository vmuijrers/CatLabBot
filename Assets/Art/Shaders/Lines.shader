// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3482,x:32719,y:32712,varname:node_3482,prsc:2|emission-1994-OUT,alpha-6637-R,voffset-2448-OUT;n:type:ShaderForge.SFN_Tex2d,id:6637,x:32247,y:32714,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_6637,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3987-UVOUT;n:type:ShaderForge.SFN_Color,id:6478,x:32247,y:32942,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_6478,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.7931032,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1994,x:32427,y:32870,varname:node_1994,prsc:2|A-6637-RGB,B-6478-RGB;n:type:ShaderForge.SFN_Time,id:4827,x:31742,y:33238,varname:node_4827,prsc:2;n:type:ShaderForge.SFN_Panner,id:3987,x:32091,y:33136,varname:node_3987,prsc:2,spu:0,spv:1|UVIN-9766-UVOUT,DIST-2729-OUT;n:type:ShaderForge.SFN_TexCoord,id:9766,x:31669,y:33040,varname:node_9766,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:526,x:31773,y:33441,ptovrint:False,ptlb:panSpeed,ptin:_panSpeed,varname:node_526,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:2729,x:31930,y:33286,varname:node_2729,prsc:2|A-4827-TSL,B-526-OUT;n:type:ShaderForge.SFN_ScreenPos,id:3101,x:31669,y:32724,varname:node_3101,prsc:2,sctp:0;n:type:ShaderForge.SFN_Abs,id:2280,x:32007,y:32790,varname:node_2280,prsc:2|IN-542-OUT;n:type:ShaderForge.SFN_Subtract,id:542,x:31838,y:32790,varname:node_542,prsc:2|A-9766-UVOUT,B-966-OUT;n:type:ShaderForge.SFN_ValueProperty,id:966,x:31601,y:32897,ptovrint:False,ptlb:node_966,ptin:_node_966,varname:node_966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_Multiply,id:2448,x:32422,y:33151,varname:node_2448,prsc:2|A-2280-OUT,B-4596-OUT,C-1720-OUT;n:type:ShaderForge.SFN_ViewVector,id:4015,x:32023,y:33380,varname:node_4015,prsc:2;n:type:ShaderForge.SFN_Cross,id:4596,x:32231,y:33414,varname:node_4596,prsc:2|A-4015-OUT,B-3849-OUT;n:type:ShaderForge.SFN_Vector3,id:3849,x:32038,y:33597,varname:node_3849,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Sign,id:1720,x:32043,y:32985,varname:node_1720,prsc:2|IN-542-OUT;proporder:6637-6478-526-966;pass:END;sub:END;*/

Shader "Custom/NewSurfaceShader" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (0,0.7931032,1,1)
        _panSpeed ("panSpeed", Float ) = 1
        _node_966 ("node_966", Float ) = 0.3
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _panSpeed;
            uniform float _node_966;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
                float2 node_542 = (o.uv0-_node_966);
                float2 node_2280 = abs(node_542);
                v.vertex.xyz += (float3(node_2280,0.0)*cross(viewDirection,float3(0,1,0))*float3(sign(node_542),0.0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
////// Lighting:
////// Emissive:
                float4 node_4827 = _Time;
                float2 node_3987 = (i.uv0+(node_4827.r*_panSpeed)*float2(0,1));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_3987, _MainTex));
                float3 emissive = (_MainTex_var.rgb*_Color.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,_MainTex_var.r);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _node_966;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
                float2 node_542 = (o.uv0-_node_966);
                float2 node_2280 = abs(node_542);
                v.vertex.xyz += (float3(node_2280,0.0)*cross(viewDirection,float3(0,1,0))*float3(sign(node_542),0.0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
