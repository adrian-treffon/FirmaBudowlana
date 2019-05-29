export interface User {
    id: string;
    firstName?: string;
    lastName?: string;
    address?: string;
    email: string;
    salt: string;
    password: string;
    role: string;
    createdAt: Date;
}