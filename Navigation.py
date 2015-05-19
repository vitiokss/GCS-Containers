import sys
import clr
import time
import MissionPlanner
clr.AddReference("MissionPlanner.Utilities") # includes the Utilities class
time.sleep(5)	# wait 10 seconds before starting
print 'Starting Drone Playback'

# https://www.google.dk/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&cad=rja&uact=8&ved=0CAcQjRw&url=http%3A%2F%2Fcopter.ardupilot.com%2Fwiki%2Finitial-setup%2Fconfiguring-hardware%2F&ei=buhDVdfLNM_6aLzygbAL&bvm=bv.92291466,bs.1,d.d2s&psig=AFQjCNHh5WdlTJzJUIzmvG7DUAtWA4VweA&ust=1430600170431516
# RC1 - roll (move left, move right)
# RC2 - pitch (go forward, go backwards)
# RC3 - throttle (go up, down)
# RC4 - yaw (turn left, right)

PITCH = 2
THROTTLE = 3
YAW = 4

# Get the pitch RC controller range values.
PITCH_MIN = Script.GetParam('RC2_MIN')
PITCH_MAX = Script.GetParam('RC2_MAX')
#PITH_STABILIZE = PITCH_MIN + ((PITCH_MAX - PITCH_MIN) / 2)
PITCH_STABILIZE = Script.GetParam("RC2_TRIM")

# Get the throttle RC controller range values.
THROTTLE_MIN = Script.GetParam('RC3_MIN')
THROTTLE_MAX = Script.GetParam('RC3_MAX')
#THROTTLE_STABILIZE = THROTTLE_MIN + ((THROTTLE_MAX - THROTTLE_MIN) / 2)
THROTTLE_STABILIZE = 1425
# Get yaw RC controller range values.
YAW_MIN = Script.GetParam('RC4_MIN')
YAW_MAX = Script.GetParam('RC4_MAX')
#YAW_STABILIZE = YAW_MIN + ((YAW_MAX - YAW_MIN) / 2)
YAW_STABILIZE = Script.GetParam("RC4_TRIM")

# Get the current done height, so based on this height,
# we will operate other commands.
DRONE_HEIGHT = cs.alt;

SINGLE_STEP = 30

log = open('log.csv', 'w')

# Take off command.
def takeoff(height):
	# requested height.
	neededHeight = DRONE_HEIGHT + float(height);
	# Arm the drone motors.
	arm()
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	# Take off and reach the needed height.
	while (cs.alt < neededHeight):
		Script.SendRC(THROTTLE, THROTTLE_STABILIZE + SINGLE_STEP, True)
	# The height reached, let's release the controller param.
	# Hold the position and release the rc.
	Script.SendRC(THROTTLE, THROTTLE_STABILIZE, False)
	Script.ChangeMode("ALTHOLD")

# Move forward, single step.
def move_forward():
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	Script.SendRC(PITCH, PITCH_STABILIZE - 100, True)
	Script.WaitFor('MOVE_FORWARD', 1000)
	# Level the pitch stick.
	Script.SendRC(PITCH, PITCH_STABILIZE, True)
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))

# Move backwards, single step.
def move_backwards():
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	Script.SendRC(PITCH, PITCH_STABILIZE + 100, True)
	Script.WaitFor('MOVE_BACKWARDS', 1000)
	# Level the pitch stick.
	Script.SendRC(PITCH, PITCH_STABILIZE, True)
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))

# Autonomously land the drone.
def mode_land():
	Script.ChangeMode('LAND')
	while (cs.alt > DRONE_HEIGHT):
		Script.WaitFor("LANDING, CURRENT HEIGHT: {0}".format(cs.alt), 1000)
	# Landed safely, now disarm the motors.
	disarm()

def mode_rtl():
	releaseRC()
	Script.ChangeMode('RTL')
	Script.WaitFor('MODE CHANGE: RTL', 1000)
	while (cs.alt > DRONE_HEIGHT):
		Script.WaitFor('RETURNING_TO_LAUNCH, CURRENT HEIGHT: {0}'.format(cs.alt), 500)
	# Landed safely, now disarm the motors.
	disarm()

# Command to turn the drone to the left.
def turn_left(angle):
	angle = float(angle)
	# Calculate the target angle in terms of current angle.
	initYaw = float(cs.yaw)
	turnedAngle = 0
	while (turnedAngle < angle):
		Script.SendRC(YAW, YAW_STABILIZE - 50, True)
		Script.WaitFor("Turn wait", 500)
		Script.SendRC(YAW, YAW_STABILIZE, True)
		currentYaw = float(cs.yaw)
		if (currentYaw > initYaw):
			turnedAngle = turnedAngle + (360 - currentYaw + initYaw)
		else:
			turnedAngle = turnedAngle + (initYaw - currentYaw)
		initYaw = currentYaw
	print "TURN LEFT"
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	Script.WaitFor("Turned left angle", 1000)

# Command to turn the drone to the right.
def turn_right(angle):
	angle = float(angle)
	# Calculate the target angle
	initYaw = float(cs.yaw)
	turnedAngle = 0
	while (turnedAngle < angle):
		Script.SendRC(YAW, YAW_STABILIZE + 50, True)
		Script.WaitFor("Turn wait", 500)
		Script.SendRC(YAW, YAW_STABILIZE, True)
		# Check if we turned over 360 degrees.
		currentYaw = float(cs.yaw)
		if (currentYaw < initYaw):
			turnedAngle = turnedAngle + (360 - initYaw + currentYaw)
		else:
			turnedAngle = turnedAngle + (currentYaw - initYaw)
		initYaw = currentYaw
	print "TURN RIGHT"
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	Script.WaitFor("Turned right angle", 1000)

# Move thSene drone up to the specific height.
def move_up(height):
	height = float(height)
	Script.ChangeMode("STABILIZE")
	while (cs.alt < height):
		Script.SendRC(THROTTLE, THROTTLE_STABILIZE + SINGLE_STEP, True)
	Script.ChangeMode("ALTHOLD")
	Script.SendRC(THROTTLE, THROTTLE_STABILIZE, True)
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	Script.WaitFor('Moved up.', 1000)

# Move the drone down to the specific height.
def move_down(height):
	height = float(height)
	Script.ChangeMode("STABILIZE")
	while(cs.alt > height):
		Script.SendRC(THROTTLE, THROTTLE_STABILIZE - SINGLE_STEP, True)
	Script.ChangeMode("ALTHOLD")
	Script.SendRC(THROTTLE, THROTTLE_STABILIZE, True)
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	Script.WaitFor("Moved down.", 1000)

# Arming drones motors.
def arm():
	print "ARMING MOTORS"
	releaseRC()
	Script.ChangeMode("STABILIZE")
	while (cs.armed == False):
		Script.SendRC(THROTTLE, THROTTLE_MIN, True)
		Script.SendRC(YAW, YAW_MAX, True)
	Script.SendRC(YAW, YAW_STABILIZE, True)
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	print "MOTORS_ARMED"

# Disarming drones motors.
def disarm():
	print "DISARMING MOTORS"
	Script.SendRC(THROTTLE, THROTTLE_MIN, False)
	Script.SendRC(YAW, YAW_MIN, True)
	Script.WaitFor('DISARMING MOTORS', 3000)
	log.write('{0}, {1}, {2} \n'.format(cs.lat, cs.lng, cs.alt))
	print "MOTORS DISARMED"

# Release all the RC overrides, so we can take over the control manually again.
def releaseRC():
	Script.SendRC(THROTTLE, 0, True)
	Script.SendRC(YAW, 0, True)
	Script.SendRC(PITCH, 0, True)

print 'Starting the mission'
# Open the commands file.
commands = [line.strip() for line in open("C:\Project\MissionPlanner\Experiments\Experiment 2\Test 1\commands.txt", "r+")]
# Loop through all the commands.
for action in commands:
	cmd = action.split()
	if cmd[0] == "TAKE_OFF":
		print "TAKEOFF"
		takeoff(cmd[1])
	elif cmd[0] == "MOVE_FORWARD":
		print "MOVE_FORWARD"
		move_forward()
	elif cmd[0] == "MOVE_BACKWARDS":
		print "MOVE_BACKWARDS"
		move_backwards()
	elif cmd[0] == "TURN_RIGHT":
		print "TURN_RIGHT"
		turn_right(cmd[1])
	elif cmd[0] == "TURN_LEFT":
		print "TURN_LEFT"
		turn_left(cmd[1])
	elif cmd[0] == "MOVE_DOWN":
		print "MOVE_DOWN"
		move_down(cmd[1])
	elif cmd[0] == "MOVE_UP":
		print "MOVE_UP"
		move_up(cmd[1])
	elif cmd[0] == "MODE_RTL":
		print "MODE_RTL"
		mode_rtl()
	elif cmd[0] == "LAND":
		land()
	elif cmd[0] == "SCAN":
		print "Hold on triggering the sensors to scan the container"

log.close()
releaseRC()
print 'Mission completed'
