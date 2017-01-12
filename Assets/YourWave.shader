Shader "Custom/YourWave"
{
	Properties{
		_MainTint("Main tint", Color) = (0,0,0,1)
		_WaveTint("Wave tint", Color) = (0,0,0,1)
		_WaveWidth("Wave width", Range(0, 1000)) = 100 
		_WaveRange("Wave range", Range(0, 1)) = 0.07			//震动幅度
		_WavePeriod("Wave period", Range(0, 1)) = 1				//震动周期
		_WaveRGBFix("Wave RGB fix", vector) = (1.9, 1, 1.5, 1)	//改变颜色(<1为其它色，>1为更白亮)
		_WaveNum("Wave num", int) = 10
		//_MainTex("Albedo (RGB)", 2D) = "white" {}
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
				fixed3 _WaveRGBFix;
				int _WaveNum;

				float _Timer;
				fixed _StepTimerMod;
				fixed4 _FixColor[20];

				int _MaxPointCount;


				//sampler2D _MainTex;

				struct vertOut {
					half4 pos:SV_POSITION;
					half4 scrPos : TEXCOORD0;
				};

				vertOut vert(appdata_base v) {
					vertOut o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.scrPos = ComputeScreenPos(o.pos);
					return o;
				}

				int findCloseIdx(fixed target)
				{
					int low = 0;
					int high = _MaxPointCount;
					int mid;
					int idx = -1;
					while(low <= high)
					{
						mid = (low + high) * 0.5;
						if(target == _FixColor[mid].w || mid == low)
						{
							idx = mid;
							break;
						}
						else if(target > _FixColor[mid].w)
						{
							low = mid;
						}
						else if(target < _FixColor[mid].w)
						{
							high = mid;
						}
					}
					if(idx + 1 < _MaxPointCount && abs(target - _FixColor[idx].w) > abs(target - _FixColor[idx + 1].w))
					{
						idx += 1;
					}
					return idx;
				}

				fixed4 frag(vertOut i) : COLOR0 
				{

					//fixed3 COLOR1 = fixed3(0.0,0.0,0.3);  
					//fixed3 COLOR2 = fixed3(0.5,0.0,0.0);  
					//float BLOCK_WIDTH = 0.03;  
					
					fixed2 uv = (i.scrPos.yx / i.scrPos.w);

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
						   uv.y += 0.00001;		//为0当除数就完了

						   fixed delta = 0.1667;	// 1 / 6.0
						   fixed t = _StepTimerMod;
						   fixed tm = fmod(t, delta);


						   for (half i = 0.0; i < _WaveNum; i++) 
						   {
							   uv.y += (_WaveRange * cos(uv.x *_WavePeriod + i * 0.1428 + _Timer));
							   wave_width = abs(1.0 / (_WaveWidth * uv.y));	//知道为什么要加0.1了吧

							   int targetIdx = findCloseIdx(uv01.x);
							   fixed target = _FixColor[targetIdx].w;
							   fixed3 fix = _FixColor[targetIdx];
							   if(uv01.x >= target)
							   {
							   		int sidx = targetIdx;
							   		int eidx = targetIdx + 1;
							   		if(eidx >= _MaxPointCount)
							   			eidx = 0;

							   		fix = lerp(_FixColor[sidx], _FixColor[eidx], (uv01.x - _FixColor[sidx].w) / (_FixColor[eidx].w - _FixColor[sidx].w) * 0.75f);
							   }
							   else 
							   {
		   					   		int sidx = targetIdx;
		   					   		int eidx = targetIdx - 1;
							   		if(eidx < 0)
							   			eidx = _MaxPointCount - 1;
				
							   		fix = lerp(_FixColor[sidx], _FixColor[eidx], (uv01.x - _FixColor[sidx].w) / (_FixColor[eidx].w - _FixColor[sidx].w) * 0.75f);
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
