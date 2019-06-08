import { Team } from './team';

export interface Order {
    orderID: string;
    startDate: Date;
    endDate: Date;
    cost: number;
    description: string;
    validated: boolean;
    paid: boolean;
    userId: string;
    teams: Team[];
}
