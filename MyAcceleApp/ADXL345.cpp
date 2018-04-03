#include "Arduino.h"
#include "ADXL345.h"
#include "Wire.h"


ADXL345::ADXL345() {};
ADXL345::~ADXL345() {};
/**************************************************************************/
/*!
@brief  Abstract away platform differences in Arduino wire library
*/
/**************************************************************************/
inline uint8_t ADXL345::i2cread(void) {
#if ARDUINO >= 100
	return Wire.read();
#else
	return Wire.receive();
#endif
}

/**************************************************************************/
/*!
@brief  Abstract away platform differences in Arduino wire library
*/
/**************************************************************************/

inline void ADXL345::i2cwrite(uint8_t x) {
#if ARDUINO >= 100
	Wire.write((uint8_t)x);
#else
	Wire.send(x);
#endif
}

uint8_t ADXL345::GetDeviceID(void)
{
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite(ADXL345_REG_DEVID);
	Wire.endTransmission();
	Wire.requestFrom(ADXL345_ADDRESS, 1);
	return (i2cread());

}

RANGE ADXL345::GetRange(void)
{
	/* Red the data format register to preserve bits */
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite(ADXL345_REG_DATA_FORMAT);
	Wire.endTransmission();
	Wire.requestFrom(ADXL345_ADDRESS, 1);

	return (RANGE)(i2cread() & 0x03);
}
DATARATE ADXL345::GetDataRate(void)
{
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite(ADXL345_REG_BW_RATE);
	Wire.endTransmission();
	Wire.requestFrom(ADXL345_ADDRESS, 0x0F);
}

int16_t ADXL345::GetX(void)
{
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite(ADXL345_REG_DATAX0);
	Wire.endTransmission();
	Wire.requestFrom(ADXL345_ADDRESS, 2);
	return (uint16_t)(i2cread() | (i2cread() << 8));
}
int16_t ADXL345::GetY(void)
{
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite(ADXL345_REG_DATAY0);
	Wire.endTransmission();
	Wire.requestFrom(ADXL345_ADDRESS, 2);
	return (uint16_t)(i2cread() | (i2cread() << 8));
}
int16_t ADXL345::GetZ(void)
{
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite(ADXL345_REG_DATAZ0);
	Wire.endTransmission();
	Wire.requestFrom(ADXL345_ADDRESS, 2);
	return (uint16_t)(i2cread() | (i2cread() << 8));
}

bool ADXL345::SetRange(RANGE range)
{
			
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite((uint8_t)ADXL345_REG_DATA_FORMAT);
	i2cwrite((uint8_t)(range));
	Wire.endTransmission();
	return true;
}

bool ADXL345::SetDataRate(DATARATE dataRate)
{
	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite((uint8_t)ADXL345_REG_BW_RATE);
	i2cwrite((uint8_t)(dataRate));
	Wire.endTransmission();
	return true;
}

bool ADXL345::Begin() {

	Wire.begin();
	/* Check connection */
	uint8_t deviceid = GetDeviceID();
	if (deviceid != 0xE5)
	{
		/* No ADXL345 detected ... return false */
		Serial.println(deviceid, HEX);
		return false;
	}

	Wire.beginTransmission(ADXL345_ADDRESS);
	i2cwrite((uint8_t)ADXL345_REG_POWER_CTL);
	i2cwrite((uint8_t)(ADXL345_MEASUREMENT_MODE));
	Wire.endTransmission();
	// Enable measurements
	return true;
}