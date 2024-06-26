﻿using ApiLibs;
using ApiLibs.General;
using System;
using System.Net;
using System.Threading.Tasks;
using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetStandard.Tradfri.Controllers
{
    public class GroupController : SubService<TradfriController>
    {
        //private readonly CoapClient cc;
        //private long id { get; }
        //private TradfriGroup group { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_id">group id</param>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load group object automatically (default: true)</param>
        public GroupController(TradfriController controller) : base(controller) { }

        /// <summary>
        /// Acquires TradfriGroup object
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <returns></returns>
        public async Task<TradfriGroup> GetTradfriGroup(long id)
        {
            return await MakeRequest<TradfriGroup>($"/{(int)TradfriConstRoot.Groups}/{id}");
        }

        /// <summary>
        /// Renames TradfriGroup object
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Task RenameTradfriGroup(TradfriGroup group)
        {
            return RenameTradfriGroup(group.ID, group.Name);
        }

        /// <summary>
        /// Renames TradfriGroup by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public Task RenameTradfriGroup(long id, string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                RenameRequest set = new RenameRequest
                {
                    Name = newName
                };
                return MakeRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set);
            }
            else
            {
                throw new Exception("Group cannot be renamed to empty string.");
            }
        }

        /// <summary>
        /// Sets a mood for the group
        /// </summary>
        /// <param name="group">A <see cref="TradfriGroup"/></param>
        /// <param name="mood">TradfriMood object which needs to be set</param>
        /// <returns></returns>
        public async Task SetMood(TradfriGroup group, TradfriMood mood)
        {
            await SetMood(group.ID, mood.ID);
            group.ActiveMood = mood.ID;
        }

        /// <summary>
        /// Set Dimmer for Light Devices in Group
        /// </summary>
        /// <param name="group">A <see cref="TradfriGroup"/></param>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public async Task SetDimmer(TradfriGroup group, int value)
        {
            await SetDimmer(group.ID, value);
            group.LightDimmer = value;
        }

        /// <summary>
        /// Sets a mood for the group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="moodId">TradfriMood ID which needs to be activated for group</param>
        /// <returns></returns>
        public async Task SetMood(long id, long moodId)
        {
            SwitchStateLightRequestOption set = new SwitchStateLightRequestOption()
            {
                IsOn = 1,
                Mood = moodId
            };
            await MakeRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set);
        }

        /// <summary>
        /// Sets a custom moodProperties for the group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="moodProperties">custom TradfriMoodProperties object which will be applied to all group bulbs</param>
        /// <returns></returns>
        public async Task SetMood(long id, TradfriMoodProperties moodProperties)
        {
            SwitchStateLightRequestOption set = new SwitchStateLightRequestOption()
            {
                IsOn = 1,
                Mood = 1 //hardcoded non-existing moodId
            };
            await MakeRequest($"/{(int)TradfriConstRoot.Groups}/{id}",
                Call.PUT,
                content: moodProperties);

            await MakeRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set);
        }

        /// <summary>
        /// Set Dimmer for Light Devices in Group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public async Task SetDimmer(long id, int value)
        {
            SwitchStateLightRequestOption set = new SwitchStateLightRequestOption()
            {
                LightIntensity = value
            };
            await MakeRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set);
        }

        /// <summary>
        /// Turns a group of lights on or off
        /// </summary>
        /// <param name="group">A <see cref="TradfriGroup"/></param>
        /// <param name="state">On (True) or Off(false)</param>
        /// <returns></returns>
        public async Task SetLight(TradfriGroup group, bool state)
        {
            await SetLight(group.ID, state);
            group.LightState = 1;
        }

        /// <summary>
        /// Turns a group of lights on or off
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="state">On (True) or Off(false)</param>
        /// <returns></returns>
        public async Task SetLight(long id, bool state)
        {
            SwitchStateLightRequestOption set = new SwitchStateLightRequestOption()
            {
                IsOn = state ? 1 : 0
            };

            await MakeRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set);
        }
    }
}
