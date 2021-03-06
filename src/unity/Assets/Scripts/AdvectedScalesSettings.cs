﻿/*
The MIT License (MIT)

Copyright (c) 2015 Huw Bowles & Daniel Zimmermann

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AdvectedScalesSettings : MonoBehaviour
{
	[Tooltip("Draw advection lines in editor")]
	public bool debugDrawAdvection = true;
	[Tooltip("A useful reference showing fixed Z and fixed R layouts")]
	public bool debugDrawAdvectionGuides = true;
	[Tooltip("Computes advection but doesn't apply it. Useful for debugging.")]
	public bool debugFreezeAdvection = false;

	[HideInInspector]
	public float debugDrawScale = 1;

	[Tooltip("Slow down unity time")]
	[Range(0.0001f,1)] public float timeScale = 1.0f;
	
	[Tooltip("Number of ray scale values used")]
	public int scaleCount = 101;

	[Tooltip("Clear ray scales every frame, with settings below")]
	public bool reInitScales = false;
	[Tooltip("When clear scales, which curvature to use")]
	[Range(0,1)] public float reInitCurvature = 0;
	[Tooltip("For fixed R layout (curvature = 1), how much noise to add")]
	[Range(0,1)] public float fixedRNoise = 0;
	[Tooltip("For fixed Z layout (curvature = 0), a simple offset useful for testing")]
	[Range(0,1)] public float fixedZProp = 0;

	[Tooltip("Clear ray scales when discontinuity in camera motion detected")]
	public bool clearOnTeleport = true;

	[Tooltip("Do advection process")]
	public bool doAdvection = true;
	[Tooltip("How many FPI iterations to perform while advecting")]
	[Range(0,15)]
	public int advectionIters = 3;

	// forward pinning moves samples inwards or outwards from the camera. for an orthognal projection this would be fine and
	// would just work. for a perspective projection the samples move not only along view z but also along view x. also
	// the speed that the sample moves depends on the ray scale which varies across the screen. luckily, the advection
	// code is able to account for all of this trivially - find usages of this variable to see the details.
	[Tooltip("When advecting, take into account sample motion due to forward pinning")]
	public bool advectionCompensatesForwardPin = true;

	[Tooltip("Whether ray scales are clamped")]
	public bool clampScaleValues = true;

	[Tooltip("Gradient relaxation enabled")]
	public bool limitGradient = true;
	[Tooltip("Max gradient target for relaxation")]
	public float maxGradient = 0.01f;
	[Tooltip("Gain control for gradient relaxation")]
	public float alphaGradient = 0.3f;

	[Tooltip("How fast newly introduced scales return to desired scale")]
	public float alphaScaleReturn = 4.0f;

	[Tooltip("Ramp up/down gradient relaxation when camera is stationary")]
	public bool useMotionMeas = true;
	[Tooltip("Coefficient for camera rotational motion")]
	public float motionMeasCoeffRot = 0.001f;
	[Tooltip("Coefficient for camera sideways motion")]
	public float motionMeasCoeffStrafe = 0.65f;

	[Tooltip("Integrate forwards motion, used for forward pinning")]
	public bool integrateForwardsMotion = true;

	[Tooltip("Enable use of two ray scales, near and far")]
	public bool twoRSolution = true;

	static AdvectedScalesSettings m_inst;
	public static AdvectedScalesSettings instance
	{
		get {
			return m_inst ? m_inst : ( m_inst = FindObjectOfType<AdvectedScalesSettings>() );
		}
	}

	void Update()
	{
		Time.timeScale = Mathf.Max(0.00001f, timeScale);
	}
}
