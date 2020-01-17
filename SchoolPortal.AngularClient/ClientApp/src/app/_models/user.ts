


// user.ts
export interface TokenData {
	isSuccess: string;
	firstName: any;
	accessToken: string;
	refreshToken: string;
	message: string;
}


export interface WeatherForecast {
	date: string;
	temperatureC: number;
	temperatureF: number;
	summary: string;
}
