import { Team } from './team';
import { User } from './user';

export interface OrderWithUser {
    orderID: string;
    startDate: Date;
    endDate: Date;
    cost: number;
    paymentCost: number;
    description: string;
    validated: boolean;
    paid: boolean;
    user: User;
    teams: Team[];
}
