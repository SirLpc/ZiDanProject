Shader "Custom/YourWave"
{
	Properties{
		_MainTint("Main tint", Color) = (0,0,0,1)
		_WaveTint("Wave tint", Color) = (0,0,0,1)
		_WaveWidth("Wave width", Range(50, 1000)) = 100 
		_WaveRange("Wave range", Range(0, 1)) = 0.07			//震动幅度
		_WavePeriod("Wave period", Range(0, 1)) = 1				//震动周期
		//_WaveRGBFix("Wave RGB fix", vector) = (1.9, 1, 1.5, 1)	//改变颜色(<1为其它色，>1为更白亮)
		_WaveNum("Wave num", int) = 10
		//_MainTex("Albedo (RGB)", 2D) = "white" {}

		_Timer("CS Timer", float) = 0.0
		_StepTimerMod("CS TimerMod", float) = 0.0
		_FixColor0("CS _FixColor0", vector) = (1,1,1,1)
		_FixColor1("CS _FixColor1", vector) = (1,1,1,1)
		_FixColor2("CS _FixColor2", vector) = (1,1,1,1)
		_FixColor3("CS _FixColor3", vector) = (1,1,1,1)
		_FixColor4("CS _FixColor4", vector) = (1,1,1,1)
		_FixColor5("CS _FixColor5", vector) = (1,1,1,1)
		_FixColor6("CS _FixColor6", vector) = (1,1,1,1)
	}

		SubShader{
			Pass {
				CGPROGRAM

				#include "UnityCG.cginc"                
				#pragma target 3.0  

				#pragma vertex vert
				#pragma fragment frag

				fixed4 _MainTint;
				fixed4 _WaveTint;
				half _WaveWidth;
				half _WaveRange;
				half _WavePeriod;
				int _WaveNum;

				float _Timer;
				fixed _StepTimerMod;
				fixed3 _FixColor0;
				fixed3 _FixColor1;
				fixed3 _FixColor2;
				fixed3 _FixColor3;
				fixed3 _FixColor4;
				fixed3 _FixColor5;
				fixed3 _FixColor6;

				//sampler2D _MainTex;

				struct vertOut {
					float4 pos:SV_POSITION;
					float4 scrPos : TEXCOORD0;
				};

				vertOut vert(appdata_base v) {
					vertOut o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.scrPos = ComputeScreenPos(o.pos);
					return o;
				}

				fixed4 frag(vertOut i) : COLOR0 {

					//fixed3 COLOR1 = fixed3(0.0,0.0,0.3);  
					//fixed3 COLOR2 = fixed3(0.5,0.0,0.0);  
					//float BLOCK_WIDTH = 0.03;  
					
					float2 uv = (i.scrPos.yx / i.scrPos.w);

					// To create the BG pattern  
					fixed3 final_color = fixed3(1.0, 1, 1);
					fixed3 bg_color = _MainTint; //fixed3(0.0, 0, 0);
					fixed3 wave_color = _WaveTint; //fixed3(0.0, 0, 0);

					/*     float c1 = fmod(uv.x, 2.0* BLOCK_WIDTH);
						   c1 = step(BLOCK_WIDTH, c1);
						   float c2 = fmod(uv.y, 2.0* BLOCK_WIDTH);
						   c2 = step(BLOCK_WIDTH, c2);
						   bg_color = lerp(uv.x * COLOR1, uv.y * COLOR2, c1*c2);  */

						   // TO create the waves   
						   half wave_width = 0.01;
						   fixed2 uv01 = uv;
						   uv = -1.0 + 2.0*uv;	//将UV区域从0到1重映射到-1到1方便操作
						   uv.y += 0.00001;		//为0当被除数就完了

						   fixed delta = 1 / 6.0;	//count 5 - 1
						   fixed t = _StepTimerMod;
						   fixed tm = fmod(t, delta);

						   for (half i = 0.0; i < _WaveNum; i++) 
						   {
							   uv.y += (_WaveRange * cos(uv.x *_WavePeriod + i / 7.0 + _Timer));
							   wave_width = abs(1.0 / (_WaveWidth * uv.y));	//知道为什么要加0.1了吧

							   fixed3 fix;

							   if(uv01.x < delta * 0 + tm)
							   {
							   		fix = _FixColor0;
						   		}	
							   	else if(uv01.x < delta * 1  + tm)
							   	{
							   		fix = _FixColor1;
							   	}
							   	else if(uv01.x < delta * 2 + tm)
							   	{
							   		fix = _FixColor2;
						   		}
							   	else if(uv01.x < delta * 3 + tm)
							   	{
							   		fix = _FixColor3;
						   		}
						   		else if(uv01.x < delta * 4 + tm)
							   	{
							   		fix = _FixColor4;
						   		}
						   		else if(uv01.x < delta * 5 + tm)
							   	{
							   		fix = _FixColor5;
						   		}
							   	else
							   	{
							   		fix = _FixColor6;
							   	}

							   wave_color += fixed3(wave_width * fix.x, wave_width * fix.y, wave_width * fix.z);

						   	}
						   final_color = bg_color + wave_color; 

						   return fixed4(final_color, 1.0);
					   }

					   ENDCG
				   }
	}
}
