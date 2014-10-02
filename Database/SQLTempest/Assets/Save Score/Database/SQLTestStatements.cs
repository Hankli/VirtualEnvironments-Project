/*
     			m_patientDB.AddPatient ("tpv", "password", "12/5/1209", "Male", "Bad");
				m_deviceDB.AddDevice ("xbox controller", "n X-BOX controller");
				m_activityDB.AddActivity ("swimming", "Swim N Drown");

				List<ReportDB.Report> list = new List<ReportDB.Report> ();
				m_reportDB.AddReport ("tpv", "xbox controller", "swimming", System.DateTime.Now, 120);
				m_reportDB.ExtractReport ("tpv", list);

				foreach(ReportDB.Report rep in list)
				{
					Debug.Log (rep.ToString() + '\n');
				}
*/


/*
				PatientDB.Patient pat = new PatientDB.Patient ();
				m_patientDB.AddPatient ("Jack012", "557632", "10/8/1995", "Male", "Bad Real Bad");
				Debug.Log(m_patientDB.FindPatient("Jack012", "557632") ? "Add Patient Success" : "Add Patient Failed");
				
				m_patientDB.UpdatePatient ("Jack012", "557632", "30/3/1975", "Female", "Average");
				m_patientDB.ReadPatient ("Jack012", "557632" ref pat);
				Debug.Log (pat.m_username + " -- " + pat.m_gender + " -- " + pat.m_medicalCondition + " -- " + pat.m_birthDate);
				
				m_patientDB.DeletePatient ("Jack012", "557632");
				Debug.Log (m_patientDB.FindPatient ("Jack012", "557632") ? "Delete Failed" : "Delete Success");
*/


/*
				DeviceDB.Device dev = new DeviceDB.Device();
				m_deviceDB.AddDevice ("Hydra", "Walking up n down!");
				Debug.Log (m_deviceDB.FindDevice ("Hydra") ? "Add Succes" : "Add Failed");

				m_deviceDB.UpdateDevice ("Hydra", "Leap", "Leap motion controller");
				Debug.Log (m_deviceDB.FindDevice ("Leap") ? "Update Success" : "Update Failed");
					
				m_deviceDB.ReadDevice ("Leap", ref dev);
				Debug.Log (dev.m_deviceName + " --- " + dev.m_description);

				m_deviceDB.DeleteDevice ("Leap");
				Debug.Log (m_deviceDB.FindDevice ("Leap") ? "Delete Failed" : "Delete Success");
*/


/*
				ActivityDBManager.Activity act = new ActivityDBManager.Activity();

				m_activityDBMgr.AddActivity ("Walk", "Walking up n down!");
				Debug.Log (m_activityDBMgr.FindActivity ("Walk") ? "Add Succes" : "Add Failed");

				m_activityDBMgr.UpdateActivity ("Walk", "Chalk", "Drawing with chalk on a chalkboard");
				Debug.Log (m_activityDBMgr.FindActivity ("Chalk") ? "Update Success" : "Update Failed");
				
				m_activityDBMgr.ReadActivity ("Chalk", ref act);
				Debug.Log (act.m_activityName + " --- " + act.m_description);

				m_activityDBMgr.DeleteActivity (act.m_activityName);
				Debug.Log (m_activityDBMgr.FindActivity (act.m_activityName) ? "Delete Failed" : "Delete Success");
*/
