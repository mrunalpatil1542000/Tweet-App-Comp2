export interface User {
	loginId: number;
	firstName: string;
	lastName: string;
	email: string;
	contactNumber: string;
}

export interface RegisterUser {
	firstName: string;
	lastName: string;
	email: string;
	password: string;
	confirmPassword: string;
	contactNumber: string;
}

export interface LoginUser {
	username: string;
	password: string;
}
