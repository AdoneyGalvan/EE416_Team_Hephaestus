#include "ADXL345.h"
#include "Arduino.h"

ADXL345 mysensor = ADXL345();
const int ACCEL_RES = 1024;         /* The ADXL345 has 10 bit resolution giving 1024 unique values                     */
const int ACCEL_DYN_RANGE_G = 8;    /* The ADXL345 had a total dynamic range of 8G, since we're configuring it to +-4G */
const int UNITS_PER_G = ACCEL_RES / ACCEL_DYN_RANGE_G;  /* Ratio of raw int values to G units                          */
const int SAMPLE_NUM = 1024;
int delay_time;

uint8_t X_0[SAMPLE_NUM];//LSB Byte X Axis
uint8_t X_1[SAMPLE_NUM];//MSB Byte X Axis
uint8_t Y_0[SAMPLE_NUM];//LSB Byte Y Axis
uint8_t Y_1[SAMPLE_NUM];//MSB Byte Y Axis
uint8_t Z_0[SAMPLE_NUM];//LSB Byte Z Axis
uint8_t Z_1[SAMPLE_NUM];//MSB Byte Z Axis

byte command = 0;
uint16_t x = 0, y = 0, z = 0;
unsigned long start, finished, elapsed;
const int loop_time = 562;

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

void displayDataRate(void)
{
	Serial.print("Data Rate: ");

	switch (mysensor.GetDataRate())
	{
	case ADXL345_DATARATE_3200_HZ:
		Serial.print("3200");
		break;
	case ADXL345_DATARATE_1600_HZ:
		Serial.print("1600");
		break;
	case ADXL345_DATARATE_800_HZ:
		Serial.print("800");
		break;
	case ADXL345_DATARATE_400_HZ:
		Serial.print("400");
		break;
	case ADXL345_DATARATE_200_HZ:
		Serial.print("200");
		break;
	case ADXL345_DATARATE_100_HZ:
		Serial.print("100");
		break;
	}
	Serial.println(" Hz");
}

void displayRange(void)
{
	Serial.print("Range: +/- ");

	switch (mysensor.GetRange())
	{
	case ADXL345_RANGE_16_G:
		Serial.print("16 ");
		break;
	case ADXL345_RANGE_8_G:
		Serial.print("8 ");
		break;
	case ADXL345_RANGE_4_G:
		Serial.print("4 ");
		break;
	case ADXL345_RANGE_2_G:
		Serial.print("2 ");
		break;
	default:
		Serial.print("?? ");
		break;
	}
	Serial.println("g");
}

void error_or_success(bool state)
{
	if (state)
	{
		//Report SUCCES ascii characters
		Serial1.write(83);
		Serial1.write(85);
		Serial1.write(67);
		Serial1.write(67);
		Serial1.write(69);
		Serial1.write(83);
	}
	else
	{
		//Report ERROR! ascii characters
		Serial1.write(69);
		Serial1.write(82);
		Serial1.write(82);
		Serial1.write(79);
		Serial1.write(82);
		Serial1.write(33);
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
		Serial.println("Command: 1 - Begin, Executed");
		break;
	case SETRANGE2G:
		state = mysensor.SetRange(ADXL345_RANGE_2_G);
		error_or_success(state);
		Serial.println("Command: 2 - Set range to +/- 2 g, Executed");
		break;
	case SETRANGE4G:
		state = mysensor.SetRange(ADXL345_RANGE_4_G);
		error_or_success(state);
		Serial.println("Command: 3 - Set range to +/- 4 g, Executed");
		break;
	case SETRANGE8G:
		state = mysensor.SetRange(ADXL345_RANGE_8_G);
		error_or_success(state);
		Serial.println("Command: 4 - Set range to +/- 8 g, Executed");
		break;
	case SETRANGE16G:
		state = mysensor.SetRange(ADXL345_RANGE_16_G);
		error_or_success(state);
		Serial.println("Command: 5 - Set range to +/- 16 g, Executed");
		break;
	case SETDATARATE100HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_100_HZ);
		delay_time = 10000 - loop_time;//In microseconds (1/100) * 1x10^6 for microseonds 
		error_or_success(state);
		Serial.println("Command: 6 - Set data rate to 100 Hz, Executed");
		break;
	case SETDATARATE200HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_200_HZ);
		delay_time = 5000 - loop_time;
		error_or_success(state);
		Serial.println("Command: 7 - Set data rate to 200 Hz, Executed");
		break;
	case SETDATARATE400HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_400_HZ);
		delay_time = 2500 - loop_time;
		error_or_success(state);
		Serial.println("Command: 8 - Set data rate to 400 Hz, Executed");
		break;
	case SETDATARATE800HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_800_HZ);
		delay_time = 1250 - loop_time;
		error_or_success(state);
		Serial.println("Command: 9 - Set data rate to 800 Hz, Executed");
		break;
	case SETDATARATE1600HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_1600_HZ);
		delay_time = 625 - loop_time;
		error_or_success(state);
		Serial.println("Command: 10 - Set data rate to 1600 Hz, Executed");
		break;
	case SETDATARATE3200HZ:
		state = mysensor.SetDataRate(ADXL345_DATARATE_3200_HZ);
		delay_time = 313;
		Serial.println("Command: 11 - Set data rate to 3200 Hz, Executed");
		error_or_success(state);
		break;
	case READDATA:
		getdata();
		Serial.println("Command: 12 - Get data, Executed");
		break;
	}

}

void getdata()
{
	//Seprate the bytes to send

	displayDataRate();
	displayRange();
	start = micros();
	for (int i = 0; i < SAMPLE_NUM; i++)
	{
		x = mysensor.GetX();
		y = mysensor.GetY();
		z = mysensor.GetZ();

		X_0[i] = (x & 0xFF00) >> 8;
		X_1[i] = (x & 0x00FF);
		Y_0[i] = (y & 0xFF00) >> 8;
		Y_1[i] = (y & 0x00FF);
		Z_0[i] = (z & 0xFF00) >> 8;
		Z_1[i] = (z & 0x00FF);
		delayMicroseconds(delay_time);

	}
	finished = micros();
	elapsed = (finished - start);
	Serial.print("Time Eslapsed: "), Serial.print(elapsed) , Serial.println(" us");

	for (int i = 0; i < SAMPLE_NUM; i++)
	{
		Serial1.write(X_0[i]);
		Serial1.write(X_1[i]);
		Serial1.write(Y_0[i]);
		Serial1.write(Y_1[i]);
		Serial1.write(Z_0[i]);
		Serial1.write(Z_1[i]);
	}
}

void setup()
{
	//Initialize the serial ports
	Serial.begin(9600);
	Serial1.begin(9600);
	//Uncommet to test 
	//mysensor.Begin();
	//mysensor.SetRange(ADXL345_RANGE_4_G);
	//mysensor.SetDataRate(ADXL345_DATARATE_1600_HZ);
	//displayDataRate();
	//displayRange();

}


void loop()
{
	while(true)
	{
		if (Serial1.available())//Check if a commands been sent 
		{
			command = Serial1.read();//Read the command
			Serial1.flush();//Clear the command buffer so the same command doesnt excute forever
		}

		execute_command(command);
		command = 0;//Reset data just ensure that 

	    //Uncommet to test
		//displayDataRate();
		//displayRange();
		//start = micros();
		//for (int i = 0; i < SAMPLE_NUM; i++)
		//{
		//	x = mysensor.GetX();
		//	y = mysensor.GetY();
		//	z = mysensor.GetZ();

		//	X_0[i] = (x & 0xFF00) >> 8;
		//	X_1[i] = (x & 0x00FF);
		//	Y_0[i] = (y & 0xFF00) >> 8;
		//	Y_1[i] = (y & 0x00FF);
		//	Z_0[i] = (z & 0xFF00) >> 8;
		//	Z_1[i] = (z & 0x00FF);
		//	delayMicroseconds(63);

		//}
		//finished = micros();
		//elapsed = (finished - start);
		//Serial.print("Time Eslapsed: "), Serial.print(elapsed), Serial.println(" us");
		//delay(1000);
		
		/*Serial.print("X: "); Serial.print((double)mysensor.GetX() / UNITS_PER_G); Serial.print("  ");
		Serial.print("Y: "); Serial.print((double)mysensor.GetY() / UNITS_PER_G); Serial.print("  ");
		Serial.print("Z: "); Serial.print((double)mysensor.GetZ() / UNITS_PER_G); Serial.print("  "); Serial.println("m/s^2 ");*/
	}
}



