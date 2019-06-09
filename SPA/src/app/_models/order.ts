import { Team } from './team';

export interface Order {
    orderID: string;
    startDate: Date;
    endDate: Date;
    cost: number;
    paymentCost: number;
    description: string;
    validated: boolean;
    paid: boolean;
    userID: string;
    teams: Team[];
}
