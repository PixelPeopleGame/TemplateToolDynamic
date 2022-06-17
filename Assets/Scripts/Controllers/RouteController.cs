using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Messages;

namespace CurrentRoute
{
    /// <summary>
    /// This class is mandatory to use, the methods within are NOT mandatory
    /// You can save stuff within classes or add methods here
    /// </summary>
    public class RouteController : Singleton<RouteController>
    {
        [field: SerializeField, Header("Route")]
        public ApiRoute Route { get; set; }

        private ApiWaypoint _currentWaypoint;

        [HideInInspector]
        public ApiWaypoint CurrentWaypoint
        {
            get { return _currentWaypoint; }
            set 
            {
                // Save BEFORE setting! d:)
                if (_currentWaypoint != null)
                    SaveCurrentWaypoint();

                _currentWaypoint = value;

                OnWaypointChanged?.Invoke();
            }
        }
        public delegate void OnVariableChangeDelegate();
        public event OnVariableChangeDelegate OnWaypointChanged;

        /// <summary>
        /// Useless function that sets the Route
        /// </summary>
        /// <param name="route">To set the Route to</param>
        public void SetCurrentRoute(ApiRoute route)
        {
            Route = route;
        }

        /// <summary>
        /// Saves the Route to the database
        /// </summary>
        public /*async*/ void SaveCurrentRoute()
        {
            // Check if new file
            //if (Route.IsNew)
            //    await ApiHandler.GetRequest($"https://{""}pixelpeople.nl/RouteFolder/generate-file?name={Route.Name}");

            // Save it all :D
            StartCoroutine(ApiHandler.PostRequest(
                $"https://{""}pixelpeople.nl/PixelPeopleAPI/RouteFolder/{Route.Name}.php",
                JsonHelper<ApiRoute>.ToJSON(Route)
                ));

            // Has to be somewhere sowwy
            // https://open.mapquestapi.com/staticmap/v5/map?key=SBEtfEnKF9W47C6Aysa38QMkxBch0iIq&center=51.429153,5.45879121986&size=1920,1920&zoom=16
            MessageBox.Instance.Show("Opgeslagen", "De route is opgeslagen naar de database!", MessageBoxType.OK);
        }

        /// <summary>
        /// Saves the CurrentWaypoint to the Route
        /// This already automatically happens upon setting the CurrentWaypoint!
        /// </summary>
        public void SaveCurrentWaypoint()
        {
            for (int i = 0; i < Route.Waypoints.Count; i++)
            {
                if (Route.Waypoints[i].Id == CurrentWaypoint.Id)
                {
                    Route.Waypoints[i] = CurrentWaypoint;
                    break;
                }
            }
        }

        /// <summary>
        /// Saves the Popup with matching Id in CurrentWaypoint
        /// </summary>
        /// <param name="popup"></param>
        public void SavePopup(ApiPopup popup)
        {
            for (int i = 0; i < CurrentWaypoint.Popups.Count; i++)
            {
                if (CurrentWaypoint.Popups[i].Id == popup.Id)
                {
                    CurrentWaypoint.Popups[i] = popup;
                    break;
                }
            }
        }

        /// <summary>
        /// Deletes a popup in CurrentWaypoint
        /// </summary>
        /// <param name="id">Id of the Popup</param>
        public void DeletePopup(int id)
        {
            for (int i = 0; i < CurrentWaypoint.Popups.Count; i++)
            {
                if (CurrentWaypoint.Popups[i].Id == id)
                {
                    CurrentWaypoint.Popups.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
