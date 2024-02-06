# unity-pong-keypoint-tracking
>This game is based on work form [Zigurous](https://www.youtube.com/@Zigurous) as seen in this [video](https://www.youtube.com/watch?v=AcpaYq0ihaM&ab_channel=Zigurous).

We expanded on the base game by adding socket capabilities to receive keypoint-data from a client. Furthermore, we added some game logic to accommodate the new controls.

Features added:
- (local) multiplayer support
- keypoint-processing
- manual start button
- sound effects
- game-over screen

# Installation
1. Install Unity version 2022.3.11f1
2. Import the project via Unity Hub

# Scripts overview
Overview of the scripts relevant to the keypoint-data and socket connection (located in Assets>Scripts):
- PoseDataUnitySocket.cs: starts the server and accepts the socket connection
- PoseDataAccess.cs: contains getter and setter for the poseData array to ensure it is thread-safe
- Paddle.cs: uses the Unity physics engine to calculate a new trajectory when the ball collides with a paddle
- LeftPlayerPaddle.cs and RightPlayerPaddle: extend Paddle.cs, calculate the velocity of the paddles for each frame using the keypoint-data
