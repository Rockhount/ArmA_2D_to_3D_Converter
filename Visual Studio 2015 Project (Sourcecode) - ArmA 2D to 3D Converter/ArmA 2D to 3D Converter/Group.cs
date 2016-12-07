using System;
using System.Collections.Generic;

namespace ArmA_2D_to_3D_Converter
{
    class Group
	{
		internal Group()
		{
			this.Vehicles = new List<Vehicle>();
			this.Waypoints = new List<Waypoint>();
		}
		//Properties--------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string Side { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public Side AssignedSide { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public List<Vehicle> Vehicles { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public List<Waypoint> Waypoints { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string[] AllCodeLines { get; set; }
	}

	class Side
	{
		internal Side(string Name, long Index)
		{
			this.Name = Name;
			this.Index = Index;
			this.AssignedGroups = new List<Group>();
		}
		//Properties--------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public string Name { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public long Index { get; set; }
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public List<Group> AssignedGroups { get; set; }
		//Methods-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public bool AutoAssignGroups ( Mission mission )
		{
			for (int i = 0; i < mission.AssignedGroups.Count; i++)
			{
				if (mission.AssignedGroups[i].Side.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
				{
					AssignedGroups.Add(mission.AssignedGroups[i]);
					mission.AssignedGroups[i].AssignedSide = this;
				}
			}
			return true;
		}
	}
}