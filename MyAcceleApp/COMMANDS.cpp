// 
// 
// 

#include "COMMANDS.h"
void execute_command(byte command)
{
	bool state = false;

	enum commands 
	{
		BEGIN = 1,
		SETRANGE2G = 2, 
		SETRANGE4G = 3,
		SETRANGE8G = 4,
		SETRANGE16G = 5,
		SETDATARATE100HZ = 6,
		SETDATARATE100HZ = 7,
		SETDATARATE200HZ = 8,
		SETDATARATE400HZ = 9,
		SETDATARATE800HZ = 10,
		SETDATARATE1600HZ = 11,
		SETDATARATE3200HZ = 12
	};

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
	}

}

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

