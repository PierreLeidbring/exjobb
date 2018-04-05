﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unidux;

namespace Pierre.Unidux {

	public class MainLightScript : MonoBehaviour {

		private Color baseColor = new Color(1,1,1,1);
		private const float intensityScaleFactor = 0.1f;
		private const float colorScaleFactor = 0.1f;
		private float cycleTime;
		private GameObject[] lightGroup;
		private float intensity = 1.0f;


		// Use this for initialization
		void Awake () {
			//timer	
			lightGroup = GameObject.FindGameObjectsWithTag("Light");
			Unidux.Subject
				.TakeUntilDisable(this)
				.StartWith(Unidux.State)
				.Subscribe(state => {
					foreach(GameObject go in lightGroup) {
						var light = go.GetComponent("Light") as Light;
						if(light.range < 100) {
							light.range += 2.0f;
						}
						
						if(light.color.r > 0.2f && light.color.g > 0.2f) {
							light.color = new Color(light.color.r - .2f, light.color.g - .2f, light.color.b, 1);
						}

						RenderSettings.ambientLight = Color.black;
					}
				})
				.AddTo(this);

		}
		
		// Update is called once per frame
		void Update () {
			if(Time.time - cycleTime > 2) {
				intensity -= intensityScaleFactor;
				Unidux.Dispatch(Actions.ActionCreator.LightIntensityAction(intensity));
				cycleTime = Time.time;
			}
		}
	}
}
