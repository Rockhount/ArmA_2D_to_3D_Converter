using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArmA_2D_to_3D_Converter
{
	class Mission
	{
		string[] GroupClassLines;
		string[] TriggerClassLines;
		string[] VehicleClassLines;
		string[] MarkerClassLines;
		string[] AllCodeLines;
		List<Marker> AssignedMarkers;
		List<Trigger> AssignedTriggers;
		List<Vehicle> AssignedVehicles;
		List<Side> AllSides;
		long CurrentUnitID, CurrentWaypointID;

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public List<Group> AssignedGroups { get; private set; }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public bool ReadSQM(string Path)
		{
			string RAWCode = File.ReadAllText(Path).Replace("\t", string.Empty);
			AllCodeLines = Regex.Split(RAWCode, "\r\n");
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private Vehicle GetVehicle(string[] VehicleClassText)
		{
			Vehicle CurrentVehicle = new Vehicle();
			for (int i = 0; i < VehicleClassText.Length; i++)
			{
				if (VehicleClassText[i].Split('=')[0].Equals("side", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.Side = VehicleClassText[i].Substring(6, VehicleClassText[i].Length - 8);
				}
				if (VehicleClassText[i].Split('=')[0].Equals("id", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.ID = Convert.ToInt64(VehicleClassText[i].Split('=')[1].Split(';')[0]);
				}
				if (VehicleClassText[i].Split('=')[0].Equals("vehicle", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.Type = VehicleClassText[i].Substring(9, VehicleClassText[i].Length - 11);
				}
				if (VehicleClassText[i].Split('=')[0].Equals("text", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.Text = VehicleClassText[i].Substring(6, VehicleClassText[i].Length - 8);
				}
				if (VehicleClassText[i].Split('=')[0].Equals("init", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.Init = VehicleClassText[i].Substring(6, VehicleClassText[i].Length - 8);
				}
				if (VehicleClassText[i].Split('=')[0].Equals("player", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.IsPlayer = true;
				}
				if (VehicleClassText[i].Split('=')[0].Equals("leader", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.IsLeader = true;
				}
				if (VehicleClassText[i].Split('=')[0].Equals("position[]", StringComparison.InvariantCultureIgnoreCase))
				{
					var CurrentPosition = new double[3];
					VehicleClassText[i] = VehicleClassText[i].Replace(" ", string.Empty);
					CurrentPosition[0] = Convert.ToDouble(VehicleClassText[i].Split(',')[0].Split('{')[1].Replace(".", ","));
					CurrentPosition[2] = Convert.ToDouble(VehicleClassText[i].Split(',')[1].Replace(".", ","));
					CurrentPosition[1] = Convert.ToDouble(VehicleClassText[i].Split(',')[2].Split('}')[0].Replace(".", ","));
					CurrentVehicle.Position = CurrentPosition;
				}
				if (VehicleClassText[i].Split('=')[0].Equals("skill", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.Skill = Convert.ToDouble(VehicleClassText[i].Split('=')[1].Split(';')[0].Replace(".", ","));
				}
				if (VehicleClassText[i].Split('=')[0].Equals("azimut", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentVehicle.Azimut = Convert.ToDouble(VehicleClassText[i].Split('=')[1].Split(';')[0].Replace(".", ","));
				}
			}
			return CurrentVehicle;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private Waypoint GetWaypoint(string[] WaypointClassText)
		{
			Waypoint CurrentWaypoint = new Waypoint();
			for (int i = 0; i < WaypointClassText.Length; i++)
			{
				if (WaypointClassText[i].Split('=')[0].Equals("placement", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.Placement = Convert.ToInt64(WaypointClassText[i].Split('=')[1].Split(';')[0]);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("combatMode", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.CombatMode = WaypointClassText[i].Substring(12, WaypointClassText[i].Length - 14);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("formation", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.Formation = WaypointClassText[i].Substring(11, WaypointClassText[i].Length - 13);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("speed", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.Speed = WaypointClassText[i].Substring(7, WaypointClassText[i].Length - 9);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("combat", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.Combat = WaypointClassText[i].Substring(8, WaypointClassText[i].Length - 10);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("description", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.Description = WaypointClassText[i].Substring(13, WaypointClassText[i].Length - 15);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("expCond", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.ExpCond = WaypointClassText[i].Substring(9, WaypointClassText[i].Length - 11);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("expActiv", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.ExpActiv = WaypointClassText[i].Substring(10, WaypointClassText[i].Length - 12);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("script", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.Script = WaypointClassText[i].Substring(8, WaypointClassText[i].Length - 10);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("timeoutMin", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.TimeoutMin = CurrentWaypoint.Placement = Convert.ToInt64(WaypointClassText[i].Split('=')[1].Split(';')[0]);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("timeoutMid", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.TimeoutMid = CurrentWaypoint.Placement = Convert.ToInt64(WaypointClassText[i].Split('=')[1].Split(';')[0]);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("timeoutMax", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.TimeoutMax = CurrentWaypoint.Placement = Convert.ToInt64(WaypointClassText[i].Split('=')[1].Split(';')[0]);
				}
				if (WaypointClassText[i].Split('=')[0].Equals("position[]", StringComparison.InvariantCultureIgnoreCase))
				{
					var CurrentPosition = new double[3];
					WaypointClassText[i] = WaypointClassText[i].Replace(" ", string.Empty);
					CurrentPosition[0] = Convert.ToDouble(WaypointClassText[i].Split(',')[0].Split('{')[1].Replace(".", ","));
					CurrentPosition[2] = Convert.ToDouble(WaypointClassText[i].Split(',')[1].Replace(".", ","));
					CurrentPosition[1] = Convert.ToDouble(WaypointClassText[i].Split(',')[2].Split('}')[0].Replace(".", ","));
					CurrentWaypoint.Position = CurrentPosition;
				}
				if (WaypointClassText[i].Split('=')[0].Equals("class Effects", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentWaypoint.AssignedEffect = GetEffect(GetClassText(WaypointClassText, i + 1, "{", "}"));
				}
			}
			return CurrentWaypoint;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private Effect GetEffect(string[] EffectClassText)
		{
			Effect CurrentEffect = new Effect();
			for (int i = 0; i < EffectClassText.Length; i++)
			{
				if (EffectClassText[i].Split('=')[0].Equals("condition", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.Condition = EffectClassText[i].Substring(11, EffectClassText[i].Length - 13);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("sound", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.Sound = EffectClassText[i].Substring(7, EffectClassText[i].Length - 9);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("soundEnv", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.SoundEnv = EffectClassText[i].Substring(10, EffectClassText[i].Length - 12);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("soundDet", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.SoundDet = EffectClassText[i].Substring(10, EffectClassText[i].Length - 12);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("voice", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.Voice = EffectClassText[i].Substring(7, EffectClassText[i].Length - 9);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("track", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.Track = EffectClassText[i].Substring(7, EffectClassText[i].Length - 9);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("titleType", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.TitleType = EffectClassText[i].Substring(11, EffectClassText[i].Length - 13);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("titleEffect", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.TitleEffect = EffectClassText[i].Substring(13, EffectClassText[i].Length - 15);
					CurrentEffect.Dummy = false;
				}
				if (EffectClassText[i].Split('=')[0].Equals("title", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentEffect.Title = EffectClassText[i].Substring(7, EffectClassText[i].Length - 9);
					CurrentEffect.Dummy = false;
				}
			}
			return CurrentEffect;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private Marker GetMarker(string[] MarkerClassText)
		{
			Marker CurrentMarker = new Marker();
			for (int i = 0; i < MarkerClassText.Length; i++)
			{
				if (MarkerClassText[i].Split('=')[0].Equals("name", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.Name = MarkerClassText[i].Substring(6, MarkerClassText[i].Length - 8);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("text", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.Text = MarkerClassText[i].Substring(6, MarkerClassText[i].Length - 8);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("fillName", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.FillName = MarkerClassText[i].Substring(10, MarkerClassText[i].Length - 12);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("type", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.Type = MarkerClassText[i].Substring(6, MarkerClassText[i].Length - 8);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("markerType", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.MarkerType = MarkerClassText[i].Substring(12, MarkerClassText[i].Length - 14);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("colorName", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.ColorName = MarkerClassText[i].Substring(11, MarkerClassText[i].Length - 13);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("a", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.A = MarkerClassText[i].Substring(2, MarkerClassText[i].Length - 3);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("b", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentMarker.B = MarkerClassText[i].Substring(2, MarkerClassText[i].Length - 3);
				}
				if (MarkerClassText[i].Split('=')[0].Equals("position[]", StringComparison.InvariantCultureIgnoreCase))
				{
					var CurrentPosition = new double[3];
					MarkerClassText[i] = MarkerClassText[i].Replace(" ", string.Empty);
					CurrentPosition[0] = Convert.ToDouble(MarkerClassText[i].Split(',')[0].Split('{')[1].Replace(".", ","));
					CurrentPosition[2] = Convert.ToDouble(MarkerClassText[i].Split(',')[1].Replace(".", ","));
					CurrentPosition[1] = Convert.ToDouble(MarkerClassText[i].Split(',')[2].Split('}')[0].Replace(".", ","));
					CurrentMarker.Position = CurrentPosition;
				}
			}
			return CurrentMarker;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private Trigger GetTrigger(string[] TriggerClassText)
		{
			Trigger CurrentTrigger = new Trigger();
			for (int i = 0; i < TriggerClassText.Length; i++)
			{
				if (TriggerClassText[i].Split('=')[0].Equals("age", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.Age = TriggerClassText[i].Substring(5, TriggerClassText[i].Length - 7);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("position[]", StringComparison.InvariantCultureIgnoreCase))
				{
					var CurrentPosition = new double[3];
					TriggerClassText[i] = TriggerClassText[i].Replace(" ", string.Empty);
					CurrentPosition[0] = Convert.ToDouble(TriggerClassText[i].Split(',')[0].Split('{')[1].Replace(".", ","));
					CurrentPosition[2] = Convert.ToDouble(TriggerClassText[i].Split(',')[1].Replace(".", ","));
					CurrentPosition[1] = Convert.ToDouble(TriggerClassText[i].Split(',')[2].Split('}')[0].Replace(".", ","));
					CurrentTrigger.Position = CurrentPosition;
				}
				if (TriggerClassText[i].Split('=')[0].Equals("a", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.A = TriggerClassText[i].Split('=')[1].Split(';')[0];
				}
				if (TriggerClassText[i].Split('=')[0].Equals("b", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.B = TriggerClassText[i].Split('=')[1].Split(';')[0];
				}
				if (TriggerClassText[i].Split('=')[0].Equals("rectangular", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.Rectangular = true;
				}
				if (TriggerClassText[i].Split('=')[0].Equals("activationBy", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.ActivationBy = TriggerClassText[i].Substring(14, TriggerClassText[i].Length - 16);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("activationType", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.ActivationType = TriggerClassText[i].Substring(16, TriggerClassText[i].Length - 18);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("repeating", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.Repeating = true;
				}
				if (TriggerClassText[i].Split('=')[0].Equals("timeoutMin", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.TimeoutMin = TriggerClassText[i].Split('=')[1].Split(';')[0];
				}
				if (TriggerClassText[i].Split('=')[0].Equals("timeoutMid", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.TimeoutMid = TriggerClassText[i].Split('=')[1].Split(';')[0];
				}
				if (TriggerClassText[i].Split('=')[0].Equals("timeoutMax", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.TimeoutMax = TriggerClassText[i].Split('=')[1].Split(';')[0];
				}
				if (TriggerClassText[i].Split('=')[0].Equals("text", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.Text = TriggerClassText[i].Substring(6, TriggerClassText[i].Length - 8);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("name", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.Name = TriggerClassText[i].Substring(6, TriggerClassText[i].Length - 8);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("expCond", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.ExpCond = TriggerClassText[i].Substring(9, TriggerClassText[i].Length - 11);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("expActiv", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.ExpActiv = TriggerClassText[i].Substring(10, TriggerClassText[i].Length - 12);
				}
				if (TriggerClassText[i].Split('=')[0].Equals("class Effects", StringComparison.InvariantCultureIgnoreCase))
				{
					CurrentTrigger.AssignedEffect = GetEffect(GetClassText(TriggerClassText, i + 1, "{", "}"));
				}
			}
			return CurrentTrigger;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private string[] GetClassText(string[] InputLines, long StartIndex, string StartSign, string EndSign)
		{
			long Count = 0;
			var Result = new List<string>();
			for (long i = StartIndex; i < InputLines.Length; i++)
			{
				Result.Add(InputLines[i]);
				Count += Regex.Matches(InputLines[i], StartSign).Count;
				Count -= Regex.Matches(InputLines[i], EndSign).Count;
				if (Count <= 0)
				{
					return Result.ToArray();
				}
			}
			return null;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public bool ProcessCode()
		{
			AssignedMarkers = new List<Marker>();
			AssignedTriggers = new List<Trigger>();
			AssignedGroups = new List<Group>();
			AssignedVehicles = new List<Vehicle>();
			for (long i = 0; i < AllCodeLines.Length; i++)
			{
				if (AllCodeLines[i].StartsWith("class Groups"))
				{
					GroupClassLines = GetClassText(AllCodeLines, i + 1, "{", "}");
				}
				if (AllCodeLines[i].StartsWith("class Sensors"))
				{
					TriggerClassLines = GetClassText(AllCodeLines, i + 1, "{", "}");
				}
				if (AllCodeLines[i].StartsWith("class Vehicles") && AllCodeLines[i - 1].EndsWith("};"))
				{
					VehicleClassLines = GetClassText(AllCodeLines, i + 1, "{", "}");
				}
				if (AllCodeLines[i].StartsWith("class Markers"))
				{
					MarkerClassLines = GetClassText(AllCodeLines, i + 1, "{", "}");
				}
			}
			Parallel.For(0, 4, new ParallelOptions { MaxDegreeOfParallelism = 4 }, i =>
			{
				switch (i)
				{
					case 0:
						if (!ReferenceEquals(GroupClassLines, null))
						{
							for (long i2 = 0; i2 < GroupClassLines.Length; i2++)
							{
								if (GroupClassLines[i2].StartsWith("class Item"))
								{
									var CurrentGroup = new Group();
									CurrentGroup.AllCodeLines = GetClassText(GroupClassLines, i2 + 1, "{", "}");
									i2 += CurrentGroup.AllCodeLines.Length;
									if (CurrentGroup.AllCodeLines.Length > 10)
									{
										AssignedGroups.Add(CurrentGroup);
									}
								}
							}
							for (int i2 = 0; i2 < AssignedGroups.Count; i2++)
							{
								for (long i3 = 0; i3 < AssignedGroups[i2].AllCodeLines.Length; i3++)
								{
									if (AssignedGroups[i2].AllCodeLines[i3].StartsWith("class Vehicles"))
									{
										var CurrentVehicleClassLines = GetClassText(AssignedGroups[i2].AllCodeLines, i3 + 1, "{", "}");
										for (int i4 = 0; i4 < CurrentVehicleClassLines.Length; i4++)
										{
											if (CurrentVehicleClassLines[i4].StartsWith("class Item"))
											{
												AssignedGroups[i2].Vehicles.Add(GetVehicle(GetClassText(CurrentVehicleClassLines, i4 + 1, "{", "}")));
												if (ReferenceEquals(AssignedGroups[i2].Side, null))
												{
													AssignedGroups[i2].Side = AssignedGroups[i2].Vehicles[0].Side;
												}
											}
										}
										i3 += CurrentVehicleClassLines.Length;
									}
									if (AssignedGroups[i2].AllCodeLines[i3].StartsWith("class Waypoints"))
									{
										var CurrentWayPointClassLines = GetClassText(AssignedGroups[i2].AllCodeLines, i3 + 1, "{", "}");
										for (int i4 = 0; i4 < CurrentWayPointClassLines.Length; i4++)
										{
											if (CurrentWayPointClassLines[i4].StartsWith("class Item"))
											{
												AssignedGroups[i2].Waypoints.Add(GetWaypoint(GetClassText(CurrentWayPointClassLines, i4 + 1, "{", "}")));
											}
										}
										i3 += CurrentWayPointClassLines.Length;
									}
								}
							}
						}
						break;
					case 1:
						if (!ReferenceEquals(TriggerClassLines, null))
						{
							for (long i2 = 0; i2 < TriggerClassLines.Length; i2++)
							{
								if (TriggerClassLines[i2].StartsWith("class Item"))
								{
									AssignedTriggers.Add(GetTrigger(GetClassText(TriggerClassLines, i2 + 1, "{", "}")));
								}
							}
						}
						break;
					case 2:
						if (!ReferenceEquals(VehicleClassLines, null))
						{
							for (long i2 = 0; i2 < VehicleClassLines.Length; i2++)
							{
								if (VehicleClassLines[i2].StartsWith("class Item"))
								{
									AssignedVehicles.Add(GetVehicle(GetClassText(VehicleClassLines, i2 + 1, "{", "}")));
								}
							}
						}
						break;
					case 3:
						if (!ReferenceEquals(MarkerClassLines, null))
						{
							for (long i2 = 0; i2 < MarkerClassLines.Length; i2++)
							{
								if (MarkerClassLines[i2].StartsWith("class Item"))
								{
									AssignedMarkers.Add(GetMarker(GetClassText(MarkerClassLines, i2 + 1, "{", "}")));
								}
							}
						}
						break;
				}
			});
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public bool WriteBIEdi(string BIEdiPath)
		{
			AllSides = new List<Side>();
			var BIEdi = new StringBuilder();
			CurrentUnitID = 0;
			CurrentWaypointID = 0;
			BIEdi.Append("class _prefix_0\r\n{\r\n\tobjectType=" + '"' + "prefix" + '"' + ";\r\n\tclass Arguments\r\n\t{\r\n\t};\r\n};\r\n");
			for (int i = 0; i < AssignedGroups.Count; i++)
			{
				if (!AllSides.Exists(s => s.Name.Equals(AssignedGroups[i].Side)))
				{
					AllSides.Add(new Side(AssignedGroups[i].Side, AllSides.Count));
					AllSides.Last().AutoAssignGroups(this);
					SetCenterBIEdiText(AllSides.Last(), BIEdi);
				}
				SetGroupBIEdiText(AssignedGroups[i], i, BIEdi);
			}
			for (int i = 0; i < AssignedMarkers.Count; i++)
			{
				SetMarkerBIEdiText(AssignedMarkers[i], i, BIEdi);
			}
			for (int i = 0; i < AssignedVehicles.Count; i++)
			{
				SetVehicleBIEdiText(AssignedVehicles[i], i, BIEdi);
			}
			for (int i = 0; i < AssignedTriggers.Count; i++)
			{
				SetTriggerBIEdiText(AssignedTriggers[i], i, BIEdi);
			}
			File.WriteAllText(BIEdiPath, BIEdi.ToString());
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private bool SetCenterBIEdiText(Side side, StringBuilder SB)
		{
			SB.Append("class _center_" + side.Index.ToString() + "\r\n");
			SB.Append("{\r\n");
			SB.Append("\tobjectType=" + '"' + "center" + '"' + ";\r\n");
			SB.Append("\tclass Arguments\r\n");
			SB.Append("\t{\r\n");
			SB.Append("\t\tSIDE=" + '"' + side.Name + '"' + ";\r\n");
			SB.Append("\t};\r\n");
			SB.Append("};\r\n");
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private bool SetGroupBIEdiText(Group group, int index, StringBuilder SB)
		{
			SB.Append("class _group_" + index.ToString() + "\r\n");
			SB.Append("{\r\n");
			SB.Append("\tobjectType=" + '"' + "group" + '"' + ";\r\n");
			SB.Append("\tclass Arguments\r\n");
			SB.Append("\t{\r\n");
			SB.Append("\t\tCENTER=" + '"' + "_center_" + group.AssignedSide.Index.ToString() + '"' + ";\r\n");
			SB.Append("\t};\r\n");
			SB.Append("};\r\n");
			for (int i = 0; i < group.Vehicles.Count; i++)
			{
				SB.Append("class _unit_" + CurrentUnitID.ToString() + "\r\n");
				SB.Append("{\r\n");
				SB.Append("\tobjectType=" + '"' + "unit" + '"' + ";\r\n");
				SB.Append("\tclass Arguments\r\n");
				SB.Append("\t{\r\n");
				SB.Append(string.Format("\t\tPOSITION={0}[{1}, {2}, {3}]{0};\r\n", '"', group.Vehicles[i].Position[0].ToString().Replace(',', '.'), group.Vehicles[i].Position[1].ToString().Replace(',', '.'), group.Vehicles[i].Position[2].ToString().Replace(',', '.')));
				SB.Append(string.Format("\t\tGROUP={0}_group_{1}{0};\r\n", '"', index.ToString()));
				SB.Append(string.Format("\t\tTYPE={0}{1}{0};\r\n", '"', group.Vehicles[i].Type));
				SB.Append(string.Format("\t\tNAME={0}{1}{0};\r\n", '"', group.Vehicles[i].Text));
				if (!ReferenceEquals(group.Vehicles[i].Init, null))
				{
					SB.Append(string.Format("\t\tINIT={0}{1}{0};\r\n", '"', group.Vehicles[i].Init));
				}
				SB.Append(string.Format("\t\tSKILL={0}{1}{0};\r\n", '"', group.Vehicles[i].Skill.ToString().Replace(',', '.')));
				if (group.Vehicles[i].IsPlayer)
				{
					SB.Append(string.Format("\t\tPLAYER={0}true{0};\r\n", '"'));
				}
				if (group.Vehicles[i].IsLeader)
				{
					SB.Append(string.Format("\t\tLEADER={0}true{0};\r\n", '"'));
				}
				SB.Append("\t};\r\n");
				SB.Append("};\r\n");
				CurrentUnitID++;
			}
			for (int i = 0; i < group.Waypoints.Count; i++)
			{
				SB.Append("class _waypoint_" + CurrentWaypointID.ToString() + "\r\n");
				SB.Append("{\r\n");
				SB.Append("\tobjectType=" + '"' + "waypoint" + '"' + ";\r\n");
				SB.Append("\tclass Arguments\r\n");
				SB.Append("\t{\r\n");
				SB.Append(string.Format("\t\tPOSITION={0}[{1}, {2}, {3}]{0};\r\n", '"', group.Waypoints[i].Position[0].ToString().Replace(',', '.'), group.Waypoints[i].Position[1].ToString().Replace(',', '.'), group.Waypoints[i].Position[2].ToString().Replace(',', '.')));
				if (!ReferenceEquals(group.Waypoints[i].Placement, null))
				{
					SB.Append(string.Format("\t\tPLACEMENT={0}{1}{0};\r\n", '"', group.Waypoints[i].Placement.ToString()));
				}
				SB.Append(string.Format("\t\tGROUP={0}_group_{1}{0};\r\n", '"', index.ToString()));
				SB.Append(string.Format("\t\tID_STATIC={0}2{0};\r\n", '"'));
				if (!ReferenceEquals(group.Waypoints[i].CombatMode, null))
				{
					SB.Append(string.Format("\t\tCOMBAT_MODE={0}{1}{0};\r\n", '"', group.Waypoints[i].CombatMode));
				}
				if (!ReferenceEquals(group.Waypoints[i].Formation, null))
				{
					SB.Append(string.Format("\t\tFORMATION={0}{1}{0};\r\n", '"', group.Waypoints[i].Formation));
				}
				if (!ReferenceEquals(group.Waypoints[i].Speed, null))
				{
					SB.Append(string.Format("\t\tSPEED={0}{1}{0};\r\n", '"', group.Waypoints[i].Speed));
				}
				if (!ReferenceEquals(group.Waypoints[i].Combat, null))
				{
					SB.Append(string.Format("\t\tCOMBAT={0}{1}{0};\r\n", '"', group.Waypoints[i].Combat));
				}
				if (!ReferenceEquals(group.Waypoints[i].Description, null))
				{
					SB.Append(string.Format("\t\tDESCRIPTION={0}{1}{0};\r\n", '"', group.Waypoints[i].Description));
				}
				if (!ReferenceEquals(group.Waypoints[i].ExpCond, null))
				{
					SB.Append(string.Format("\t\tEXP_COND={0}{1}{0};\r\n", '"', group.Waypoints[i].ExpCond));
				}
				if (!ReferenceEquals(group.Waypoints[i].ExpActiv, null))
				{
					SB.Append(string.Format("\t\tEXP_ACTIV={0}{1}{0};\r\n", '"', group.Waypoints[i].ExpActiv));
				}
				if (!ReferenceEquals(group.Waypoints[i].Script, null))
				{
					SB.Append(string.Format("\t\tSCRIPT={0}{1}{0};\r\n", '"', group.Waypoints[i].Script));
				}
				if (!ReferenceEquals(group.Waypoints[i].TimeoutMin, null))
				{
					SB.Append(string.Format("\t\tTIMEOUT_MIN={0}{1}{0};\r\n", '"', group.Waypoints[i].TimeoutMin.ToString()));
				}
				if (!ReferenceEquals(group.Waypoints[i].TimeoutMid, null))
				{
					SB.Append(string.Format("\t\tTIMEOUT_MID={0}{1}{0};\r\n", '"', group.Waypoints[i].TimeoutMid.ToString()));
				}
				if (!ReferenceEquals(group.Waypoints[i].TimeoutMax, null))
				{
					SB.Append(string.Format("\t\tTIMEOUT_MAX={0}{1}{0};\r\n", '"', group.Waypoints[i].TimeoutMax.ToString()));
				}
				if (!group.Waypoints[i].AssignedEffect.Dummy)
				{
					SetEffectBIEdiText(group.Waypoints[i].AssignedEffect, SB);
				}
				SB.Append("\t};\r\n");
				SB.Append("};\r\n");
				CurrentWaypointID++;
			}
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private bool SetMarkerBIEdiText(Marker marker, int index, StringBuilder SB)
		{
			SB.Append("class _marker_" + index.ToString() + "\r\n");
			SB.Append("{\r\n");
			SB.Append("\tobjectType=" + '"' + "marker" + '"' + ";\r\n");
			SB.Append("\tclass Arguments\r\n");
			SB.Append("\t{\r\n");
			SB.Append(string.Format("\t\tPOSITION={0}[{1}, {2}, {3}]{0};\r\n", '"', marker.Position[0].ToString().Replace(',', '.'), marker.Position[1].ToString().Replace(',', '.'), marker.Position[2].ToString().Replace(',', '.')));
			SB.Append(string.Format("\t\tNAME={0}{1}{0};\r\n", '"', marker.Name));
			if (!ReferenceEquals(marker.Text, null))
			{
				SB.Append(string.Format("\t\tTEXT={0}{1}{0};\r\n", '"', marker.Text));
			}
			if (!ReferenceEquals(marker.MarkerType, null))
			{
				SB.Append(string.Format("\t\tMARKER_TYPE={0}{1}{0};\r\n", '"', marker.MarkerType));
			}
			if (!marker.Type.Equals("mil_objective", StringComparison.OrdinalIgnoreCase))
			{
				SB.Append(string.Format("\t\tTYPE={0}{1}{0};\r\n", '"', marker.Type));
			}
			if (!ReferenceEquals(marker.ColorName, null))
			{
				SB.Append(string.Format("\t\tCOLOR={0}{1}{0};\r\n", '"', marker.ColorName));
			}
			SB.Append(string.Format("\t\tFILL={0}{1}{0};\r\n", '"', marker.FillName));
			if (!ReferenceEquals(marker.A, null))
			{
				SB.Append(string.Format("\t\tA={0}{1}{0};\r\n", '"', marker.A));
			}
			if (!ReferenceEquals(marker.B, null))
			{
				SB.Append(string.Format("\t\tB={0}{1}{0};\r\n", '"', marker.B));
			}
			SB.Append("\t};\r\n");
			SB.Append("};\r\n");
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private bool SetVehicleBIEdiText(Vehicle vehicle, int index, StringBuilder SB)
		{
			SB.Append("class _vehicle_" + index.ToString() + "\r\n");
			SB.Append("{\r\n");
			SB.Append("\tobjectType=" + '"' + "vehicle" + '"' + ";\r\n");
			SB.Append("\tclass Arguments\r\n");
			SB.Append("\t{\r\n");
			SB.Append(string.Format("\t\tPOSITION={0}[{1}, {2}, {3}]{0};\r\n", '"', vehicle.Position[0].ToString().Replace(',', '.'), vehicle.Position[1].ToString().Replace(',', '.'), vehicle.Position[2].ToString().Replace(',', '.')));
			SB.Append(string.Format("\t\tTYPE={0}{1}{0};\r\n", '"', vehicle.Type));
			if (!ReferenceEquals(vehicle.Azimut, null))
			{
				SB.Append(string.Format("\t\tAZIMUT={0}{1}{0};\r\n", '"', vehicle.Azimut.ToString().Replace(',', '.')));
			}
			SB.Append(string.Format("\t\tNAME={0}{1}{0};\r\n", '"', vehicle.Text));
			if (!ReferenceEquals(vehicle.Init, null))
			{
				SB.Append(string.Format("\t\tINIT={0}{1}{0};\r\n", '"', vehicle.Init));
			}
			SB.Append(string.Format("\t\tPARENT={0}{0};\r\n", '"'));
			SB.Append("\t};\r\n");
			SB.Append("};\r\n");
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private bool SetTriggerBIEdiText(Trigger trigger, int index, StringBuilder SB)
		{
			SB.Append("class _trigger_" + index.ToString() + "\r\n");
			SB.Append("{\r\n");
			SB.Append("\tobjectType=" + '"' + "trigger" + '"' + ";\r\n");
			SB.Append("\tclass Arguments\r\n");
			SB.Append("\t{\r\n");
			SB.Append(string.Format("\t\tPOSITION={0}[{1}, {2}, {3}]{0};\r\n", '"', trigger.Position[0].ToString().Replace(',', '.'), trigger.Position[1].ToString().Replace(',', '.'), trigger.Position[2].ToString().Replace(',', '.')));
			SB.Append(string.Format("\t\tTYPE={0}GUER G{0};\r\n", '"'));
			if (!ReferenceEquals(trigger.A, null))
			{
				SB.Append(string.Format("\t\tA={0}{1}{0};\r\n", '"', trigger.A));
			}
			if (!ReferenceEquals(trigger.B, null))
			{
				SB.Append(string.Format("\t\tB={0}{1}{0};\r\n", '"', trigger.B));
			}
			if (trigger.Rectangular)
			{
				SB.Append(string.Format("\t\tRECTANGULAR={0}true{0};\r\n", '"'));
			}
			if (!ReferenceEquals(trigger.ActivationBy, null))
			{
				SB.Append(string.Format("\t\tACTIVATION_BY={0}{1}{0};\r\n", '"', trigger.ActivationBy));
			}
			if (!ReferenceEquals(trigger.ActivationType, null))
			{
				SB.Append(string.Format("\t\tACTIVATION_TYPE={0}{1}{0};\r\n", '"', trigger.ActivationType));
			}
			if (trigger.Repeating)
			{
				SB.Append(string.Format("\t\tREPEATING={0}true{0};\r\n", '"'));
			}
			if (!ReferenceEquals(trigger.TimeoutMin, null))
			{
				SB.Append(string.Format("\t\tTIMEOUT_MIN={0}{1}{0};\r\n", '"', trigger.TimeoutMin));
			}
			if (!ReferenceEquals(trigger.TimeoutMid, null))
			{
				SB.Append(string.Format("\t\tTIMEOUT_MID={0}{1}{0};\r\n", '"', trigger.TimeoutMid));
			}
			if (!ReferenceEquals(trigger.TimeoutMax, null))
			{
				SB.Append(string.Format("\t\tTIMEOUT_MAX={0}{1}{0};\r\n", '"', trigger.TimeoutMax));
			}
			if (trigger.Interruptable)
			{
				SB.Append(string.Format("\t\tINTERRUPTABLE={0}true{0};\r\n", '"'));
			}
			if (!ReferenceEquals(trigger.Text, null))
			{
				SB.Append(string.Format("\t\tTEXT={0}{1}{0};\r\n", '"', trigger.Text));
			}
			if (!ReferenceEquals(trigger.Name, null))
			{
				SB.Append(string.Format("\t\tNAME={0}{1}{0};\r\n", '"', trigger.Name));
			}
			if (!ReferenceEquals(trigger.ExpCond, null))
			{
				SB.Append(string.Format("\t\tEXP_COND={0}{1}{0};\r\n", '"', trigger.ExpCond));
			}
			if (!ReferenceEquals(trigger.ExpActiv, null))
			{
				SB.Append(string.Format("\t\tEXP_ACTIV={0}{1}{0};\r\n", '"', trigger.ExpActiv));
			}
			if (!ReferenceEquals(trigger.ExpDesactiv, null))
			{
				SB.Append(string.Format("\t\tEXP_DESACTIV={0}{1}{0};\r\n", '"', trigger.ExpDesactiv));
			}
			if (!trigger.AssignedEffect.Dummy)
			{
				SetEffectBIEdiText(trigger.AssignedEffect, SB);
			}
			SB.Append("\t};\r\n");
			SB.Append("};\r\n");
			return true;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		private bool SetEffectBIEdiText(Effect effect, StringBuilder SB)
		{
			if (!ReferenceEquals(effect.Condition, null))
			{
				SB.Append(string.Format("\t\tEFFECT_CONDITION={0}{1}{0};\r\n", '"', effect.Condition));
			}
			if (!ReferenceEquals(effect.Sound, null))
			{
				SB.Append(string.Format("\t\tSOUND_EFFECT={0}{1}{0};\r\n", '"', effect.Sound));
			}
			if (!ReferenceEquals(effect.SoundEnv, null))
			{
				SB.Append(string.Format("\t\tSOUND_ENV_EFFECT={0}{1}{0};\r\n", '"', effect.SoundEnv));
			}
			if (!ReferenceEquals(effect.SoundDet, null))
			{
				SB.Append(string.Format("\t\tSOUND_DET_EFFECT={0}{1}{0};\r\n", '"', effect.SoundDet));
			}
			if (!ReferenceEquals(effect.Voice, null))
			{
				SB.Append(string.Format("\t\tVOICE_EFFECT={0}{1}{0};\r\n", '"', effect.Voice));
			}
			if (!ReferenceEquals(effect.Track, null))
			{
				SB.Append(string.Format("\t\tMUSIC_EFFECT={0}{1}{0};\r\n", '"', effect.Track));
			}
			if (!ReferenceEquals(effect.TitleType, null))
			{
				SB.Append(string.Format("\t\tTITLE_EFFECT_TYPE={0}{1}{0};\r\n", '"', effect.TitleType));
			}
			if (!ReferenceEquals(effect.TitleEffect, null))
			{
				SB.Append(string.Format("\t\tTITLE_EFFECT_EFFECT={0}{1}{0};\r\n", '"', effect.TitleEffect));
			}
			if (!ReferenceEquals(effect.Title, null))
			{
				SB.Append(string.Format("\t\tTITLE_EFFECT_TITLE={0}{1}{0};\r\n", '"', effect.Title));
			}
			return true;
		}
	}
}