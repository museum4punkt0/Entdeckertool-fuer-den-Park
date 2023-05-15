Shader "custom/stencilWrite"{
	//show values to edit in inspector
	Properties{
		[IntRange] _StencilRef ("Stencil Reference Value", Range(0,255)) = 0
	}

	SubShader{
		//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
		Tags{ "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

        //stencil operation


		Pass{
            //don't draw color or depth
			Blend Zero One
			ZWrite Off

			Stencil{
				Ref[_StencilRef]
				Comp Always
				Pass Replace
				Fail Keep
			}

			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			struct appdata{
				float4 vertex : POSITION;
			};

			struct v2f{
				float4 position : SV_POSITION;
			};

			v2f vert(appdata v){
				v2f o;
				//calculate the position in clip space to render the object
				o.position = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				return 0;
			}

			ENDCG
		}
	}
}