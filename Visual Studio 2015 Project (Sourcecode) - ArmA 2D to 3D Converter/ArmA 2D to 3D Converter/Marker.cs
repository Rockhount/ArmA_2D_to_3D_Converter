﻿namespace ArmA_2D_to_3D_Converter
{
    class Marker
	{
		internal Marker()
		{
			this.FillName = "Solid";
			this.Type = "mil_objective";
			this.Name = string.Empty;
		}
		//Properties--------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double[] Position { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string Name { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string FillName { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string Type { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string MarkerType { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string Text { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string ColorName { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string A { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string B { get; set; }
	}
}