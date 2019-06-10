import { Team } from './team';

export interface Worker {
    workerID: string;
    firstName: string;
    lastName: string;
    specialization: string;
    manHour: number;
    teams: Team[];
}
