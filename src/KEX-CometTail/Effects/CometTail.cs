﻿using Kopernicus.Components;
using Kopernicus.Constants;
using KopernicusExpansion.Geometry;
using UnityEngine;
using UnityEngine.Rendering;

namespace KopernicusExpansion
{
    namespace CometTail
    {
        namespace Effects
        {
            /// <summary>
            /// The Class implementing an API for adding comet tails
            /// </summary>
            public class Tail
            {
                public CometTailType type;

                public Color color = Color.white;
                public float rimPower = 1.41f;
                public float distortion = 0.143f;
                public float alphaDistortion = 0.262f;
                public float zDistortion = 0.12f;
                public float frequency = 1.547f;
                public float lacunarity = 1.518f;
                public float gain = 0.734f;

                public float radius = 2000f;
                public float length = 16000f;
                public FloatCurve opacityCurve;
                public FloatCurve brightnessCurve;

                private const string FallbackShader = "Particles/Alpha Blended";

                public static void AddCometTail(PSystemBody body, Tail tail)
                {
                    Transform scaledVersion = body.scaledVersion.transform;

                    GameObject obj = new GameObject("CometTail");
                    obj.layer = GameLayers.ScaledSpace;
                    obj.transform.parent = Kopernicus.Utility.Deactivator;

                    MeshRenderer mr = obj.AddComponent<MeshRenderer>();
                    MeshFilter mf = obj.AddComponent<MeshFilter>();

                    Teardrop teardrop = new Teardrop(1f, (tail.length / tail.radius), 60, 90);
                    mf.mesh = teardrop;

                    mr.sharedMaterial = new Material(ShaderLoader.GetShader("KopernicusExpansion/CometTail"));

                    //set default material values
                    mr.sharedMaterial.SetColor("_TintColor", new Color(tail.color.r, tail.color.g, tail.color.b, 0.5f));

                    mr.sharedMaterial.SetFloat("_RimPower", tail.rimPower);
                    mr.sharedMaterial.SetFloat("_Distortion", tail.distortion);
                    mr.sharedMaterial.SetFloat("_AlphaDistortion", tail.alphaDistortion);
                    mr.sharedMaterial.SetFloat("_ZDistortion", tail.zDistortion);
                    mr.sharedMaterial.SetFloat("_VertexDistortion", 0f);
                    mr.sharedMaterial.SetFloat("_Frequency", tail.frequency);
                    mr.sharedMaterial.SetFloat("_Lacunarity", tail.lacunarity);
                    mr.sharedMaterial.SetFloat("_Gain", tail.gain);

                    mr.shadowCastingMode = ShadowCastingMode.Off;
                    mr.receiveShadows = false;

                    CometTailController cometController = obj.AddComponent<CometTailController>();
                    cometController.type = tail.type;
                    cometController.color = tail.color;
                    cometController.orbit = body.celestialBody.orbit;
                    cometController.opacityCurve = tail.opacityCurve;
                    cometController.brightnessCurve = tail.brightnessCurve;

                    obj.transform.parent = scaledVersion;
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = (Vector3.one * tail.length);
                    obj.SetActive(true);
                    obj.layer = GameLayers.ScaledSpace;
                }

            }
        }
    }
}