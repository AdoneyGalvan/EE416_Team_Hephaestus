#include "COMMANDS.h"
#include "ADXL345.h"
#include "Arduino.h"
#include <SoftwareSerial.h>

ADXL345 mysensor = ADXL345();
SoftwareSerial BTSerial(8, 7); // Arduino RX, Arduino TX
const int ACCEL_RES = 1024;         /* The ADXL345 has 10 bit resolution giving 1024 unique values                     */
const int ACCEL_DYN_RANGE_G = 8;    /* The ADXL345 had a total dynamic range of 8G, since we're configuring it to +-4G */
const int UNITS_PER_G = ACCEL_RES / ACCEL_DYN_RANGE_G;  /* Ratio of raw int values to G units                          */


enum commands
{
	BEGIN = 1,
	SETRANGE2G = 2,
	SETRANGE4G = 3,
	SETRANGE8G = 4,
	SETRANGE16G = 5,
	SETDATARATE100HZ = 6,
	SETDATARATE200HZ = 7,
	SETDATARATE400HZ = 8,
	SETDATARATE800HZ = 9,
	SETDATARATE1600HZ = 10,
	SETDATARATE3200HZ = 11,
	READDATA = 12,
};

void error_or_success(bool state)
{
	if (state)
	{
		//Report SUCCES ascii characters
		BTSerial.write(83);
		BTSerial.write(85);
		BTSerial.write(67);
		BTSerial.write(67);
		BTSerial.write(69);
		BTSerial.write(83);
	}
	else
	{
		//Report ERROR! ascii characters
		BTSerial.write(69);
		BTSerial.write(82);
		BTSerial.write(82);
		BTSerial.write(79);
		BTSerial.write(82);
		BTSerial.write(33);
	}
}

void execute_command(byte command)
{
	bool state = false;

	switch (command)
	{
	case BEGIN:
		state = mysensor.Begin();
		error_or_success(state);
		break;
	case SETRANGE2G:
		state = mysensor.SetRange(ADXL345_RANGE_2_G);
		error_or_success(state);
		break;
	case SETRANGE4G:
		state = mysensor.SetRange(ADXL345_RANGE_4_G);
		error_or_success(state);
		break;
	case SETRANGE8G:
		state = mysensor.SetRange(ADXL345_RANGE_8_G);
		error_or_success(state);
		break;
	case SETRANGE16G:
		state = mysensor.SetRange(ADXL345_RANGE_16_G);
		error_or_success(state);
		break;
	case SETDATARATE100HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_100_HZ);
		error_or_success(state);
		break;
	case SETDATARATE200HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_200_HZ);
		error_or_success(state);
		break;
	case SETDATARATE400HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_400_HZ);
		error_or_success(state);
		break;
	case SETDATARATE800HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_800_HZ);
		error_or_success(state);
		break;
	case SETDATARATE1600HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_1600_HZ);
		error_or_success(state);
		break;
	case SETDATARATE3200HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_3200_HZ);
		error_or_success(state);
		break;
	case READDATA:
		getdata();
		break;
	}

}

void getdata()
{
	uint16_t x = 0, y = 0, z = 0;
	byte x_1 = 0, x_2 = 0, y_1 = 0, y_2 = 0, z_1 = 0, z_2 = 0;
	//Seprate the bytes to send
	x = mysensor.GetX();
	y = mysensor.GetY();
	z = mysensor.GetZ();

	x_1 = (x & 0xFF00) >> 8;
	x_2 = (x & 0x00FF);
	y_1 = (y & 0xFF00) >> 8;
	y_2 = (y & 0x00FF);
	z_1 = (z & 0xFF00) >> 8;
	z_2 = (z & 0x00FF);

	//Write the data no need for a success flag
	BTSerial.write(x_1);
	BTSerial.write(x_2);
	BTSerial.write(y_1);
	BTSerial.write(y_2);
	BTSerial.write(z_1);
	BTSerial.write(z_2);
}

void setup()
{
	//Initialize the serial ports
	Serial.begin(9600);
	BTSerial.begin(9600);
	//mysensor.Begin();//Uncommet to test 
}


void loop()
{
	byte command = 0;
	while(true)
	{
		if (BTSerial.available())//Check if a commands been sent 
		{
			command = BTSerial.read();//Read the command
			BTSerial.flush();//Clear the command buffer so the same command doesnt excute forever
			Serial.println(command);//Print it to the serial comport to view the command
		}

		execute_command(command);
		command = 0;//Reset data just ensure that 

		//Uncommet to test
		//Serial.print("X: "); Serial.print((double)mysensor.GetX() / UNITS_PER_G); Serial.print("  ");
		//Serial.print("Y: "); Serial.print((double)mysensor.GetY() / UNITS_PER_G); Serial.print("  ");
		//Serial.print("Z: "); Serial.print((double)mysensor.GetZ() / UNITS_PER_G); Serial.print("  "); Serial.println("m/s^2 ");
	}
}



