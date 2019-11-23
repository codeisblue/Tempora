using System;
using System.Collections.Generic;
using System.Text;
using Tempora.Engine.Entities;
using Microsoft.Xna.Framework;

namespace Tempora.Engine
{
    /// <summary>
    /// A static class that handles cameras
    /// </summary>
    public class CameraManager
    {
        /// <summary>
        /// The current active camera
        /// </summary>
        public static ECamera ActiveCamera;

        //internal values
        private static Matrix DefaultCamera = new Matrix();
        private static bool setupMatrix = false;

        /// <summary>
        /// Changes the active rendering camera
        /// </summary>
        /// <param name="c">The camera to change it to</param>
        public static void SetActiveCamera(ECamera c)
        {
            ActiveCamera = c;
        }

        /// <summary>
        /// Returns the current active matrix on the camera
        /// This is used by the render engine to offset the drawing space to match the physical cameras properties
        /// </summary>
        /// <returns>The Matrix</returns>
        public static Matrix GetActiveMatrix()
        {
            //If we don't have a matrix set up, then lets just use the default settings for a camera
            if (!setupMatrix)
            {
                setupMatrix = true;
                DefaultCamera = Matrix.CreateScale(1);
                DefaultCamera.Translation = new Vector3(0,0,0);
            }

            //Do we currently have an active camera?
            if(ActiveCamera != null)
            {
                
                //Calculate the correct scale based on our aspect ratio
                float ratio = (float)GameManager.GraphicsDeviceInstance.Viewport.Width / (float)GameManager.GraphicsDeviceInstance.Viewport.Height;
                float scaleX = (float)GameManager.GraphicsDeviceInstance.Viewport.Width / (float)(ActiveCamera.transform.Scale.X * ratio);
                float scaleY = (float)GameManager.GraphicsDeviceInstance.Viewport.Height / (float)(ActiveCamera.transform.Scale.X);

                //Create the matrix that represents the cameras rotation, location and scale
                Matrix scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));
                Matrix position = Matrix.CreateTranslation(new Vector3(-ActiveCamera.transform.Position.X, -ActiveCamera.transform.Position.Y, 0));
                Matrix rotation = Matrix.CreateRotationZ(ActiveCamera.transform.Rotation);
                Matrix scaleOffset = Matrix.CreateTranslation(new Vector3(GameManager.GraphicsDeviceInstance.Viewport.Width / 2, GameManager.GraphicsDeviceInstance.Viewport.Height / 2, 0));

                //Return it
                return (position * rotation * scale) * scaleOffset;
            }

            //If we got this far then there is no camera so return the default camera settings
            return DefaultCamera;
        }
    }
}
