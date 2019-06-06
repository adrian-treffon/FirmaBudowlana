import { Worker } from '../_models/worker';

export interface Team {
    teamID: string;
    description: string;
    workers: Worker[];
}
