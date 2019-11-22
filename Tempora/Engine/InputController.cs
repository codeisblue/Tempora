using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tempora.Engine
{
    /// <summary>
    /// Represents a particular input event
    /// </summary>
    public enum InputAction
    {
        //Misc
        QUIT = 0,
        BACK = 1,

        //Movement
        LEFT = 100,
        RIGHT = 101,
        FORWARD = 102,
        BACKWARD = 103,
        JUMP = 200,
        CROUCH = 300,
        RUN = 350,

        //Firing
        FIRE = 400,
        SECONDARY_FIRE = 401,

        //Mouse wheel
        MWHEEL_UP = 500,
        MWHEEL_DOWN = 501
    }

    /// <summary>
    /// Represents the type of event that happened
    /// </summary>
    public enum InputEventType
    {
        Pressed = 0,        //The key was pressed this frame
        Held = 1,           //The key was held for more than 1 frame
        Released = 2,       //The key was released
        Stream = 10         //Used for things like joysticks or values that are non binary
    }

    /// <summary>
    /// Represents mouse buttons
    /// </summary>
    public enum MouseButtons
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    /// <summary>
    /// A class for handling input from the user
    /// An input controller can posseses possesable entities and feed intput events to the such as a player, car or controllable NPC
    /// </summary>
    public class InputController
    {

        /// <summary>
        /// The entity we are currently possesing
        /// </summary>
        public Entity possesedEntity;

        /// <summary>
        /// The states of all the keys on the keyboard (from the previous frame)
        /// </summary>
        private Dictionary<Keys, bool> KeyStates = new Dictionary<Keys, bool>();

        /// <summary>
        /// The states of all the mouse buttons (from the previous frame)
        /// </summary>
        private Dictionary<MouseButtons, bool> MouseStates = new Dictionary<MouseButtons, bool>();

        /// <summary>
        /// The last known scroll wheen value, used to keep track of scrolls
        /// </summary>
        private int previousMWheelValue = 0;

        /// <summary>
        /// Set up key states
        /// </summary>
        public InputController()
        {
            for(int i = 0; i < 256; i++)
                KeyStates.Add((Keys)i, false);

            for (int i = 0; i < 3; i++)
                MouseStates.Add((MouseButtons)i, false);
        }

        /// <summary>
        /// Will fire the input event to the possesed entity if one is possesed
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="type">The type</param>
        /// <param name="metaData">Metadata</param>
        /// <param name="gameTime">Game Time</param>
        public void TriggerInputEvent(InputAction action, InputEventType type, float metaData, GameTime gameTime)
        {
            if (possesedEntity != null)
                possesedEntity.OnInputEvent(action, type, metaData, gameTime);
        }

        /// <summary>
        /// Checks the state of a key, and will call TriggerInput based on the result with action
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <param name="action">The action to perform</param>
        /// <param name="gameTime">Game Time</param>
        public void CheckKeyState(Keys key, InputAction action, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                if (!KeyStates[key])
                {
                    KeyStates[key] = true;
                    TriggerInputEvent(action, InputEventType.Pressed, 0, gameTime);
                }
                else
                    TriggerInputEvent(action, InputEventType.Held, 0, gameTime);
            }
            else if (KeyStates[key])
            {
                KeyStates[key] = false;
                TriggerInputEvent(action, InputEventType.Released, 0, gameTime);
            }
        }

        /// <summary>
        /// Checks the state of a mouse button, and will call TriggerInput based on the result with action
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <param name="action">The action to perform</param>
        /// <param name="gameTime">Game Time</param>
        public void CheckMouseState(MouseButtons button, InputAction action, GameTime gameTime)
        {
            bool state = false;

            switch (button)
            {
                case MouseButtons.Left: state = Mouse.GetState().LeftButton == ButtonState.Pressed; break; //Left mouse button
                case MouseButtons.Right: state = Mouse.GetState().RightButton == ButtonState.Pressed; break; //Left mouse button
                case MouseButtons.Middle: state = Mouse.GetState().MiddleButton == ButtonState.Pressed; break; //Left mouse button
            }

            if (state)
            {
                if (!MouseStates[button])
                {
                    MouseStates[button] = true;
                    TriggerInputEvent(action, InputEventType.Pressed, 0, gameTime);
                }
                else
                    TriggerInputEvent(action, InputEventType.Held, 0, gameTime);
            }
            else if (MouseStates[button])
            {
                MouseStates[button] = false;
                TriggerInputEvent(action, InputEventType.Released, 0, gameTime);
            }
        }

        /// <summary>
        /// Ticks the input controller, checking the state of all keys and buttons
        /// </summary>
        /// <param name="gameTime"></param>
        public void TickInput(GameTime gameTime)
        {
            //Misc
            CheckKeyState(Keys.Escape, InputAction.QUIT, gameTime);
            CheckKeyState(Keys.Back, InputAction.BACK, gameTime);

            //Movement
            CheckKeyState(Keys.W, InputAction.FORWARD, gameTime);
            CheckKeyState(Keys.A, InputAction.LEFT, gameTime);
            CheckKeyState(Keys.S, InputAction.BACKWARD, gameTime);
            CheckKeyState(Keys.D, InputAction.RIGHT, gameTime);
            CheckKeyState(Keys.Space, InputAction.JUMP, gameTime);
            CheckKeyState(Keys.LeftControl, InputAction.CROUCH, gameTime);
            CheckKeyState(Keys.LeftShift, InputAction.RUN, gameTime);

            //Firing
            CheckMouseState(MouseButtons.Left, InputAction.FIRE, gameTime);
            CheckMouseState(MouseButtons.Right, InputAction.SECONDARY_FIRE, gameTime);

            //Check the mouse scroll state
            if(Mouse.GetState().ScrollWheelValue > previousMWheelValue)
                TriggerInputEvent(InputAction.MWHEEL_UP, InputEventType.Stream, Mouse.GetState().ScrollWheelValue, gameTime);

            if (Mouse.GetState().ScrollWheelValue < previousMWheelValue)
                TriggerInputEvent(InputAction.MWHEEL_DOWN, InputEventType.Stream, Mouse.GetState().ScrollWheelValue, gameTime);

            previousMWheelValue = Mouse.GetState().ScrollWheelValue;
        }

        /// <summary>
        /// Attempts to posses the entity
        /// </summary>
        /// <param name="e">The entity to posses</param>
        /// <returns>Did we posses the entity?</returns>
        public bool Posses(Entity e)
        {
            if (e.Possesable)
                if (e.OnPossesed(this))
                {
                    possesedEntity = e;
                    return true;
                }
                else
                    Console.WriteLine("Entity does not want to be possesed");
            else
                Console.WriteLine("Warning! Tried to posses and entity which is not possesable!");


            return false;
        }


    }
}
