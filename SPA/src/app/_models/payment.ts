import { Team } from './team';

export interface Payment {
    orderID: string;
    description: string;
    teams: Team[];
    paymentCost: number;
    startDate: Date;
    endDate: Date;
}
