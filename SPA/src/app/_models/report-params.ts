import { Team } from './team';
import { Worker } from './worker';

export interface ReportParams {
    startDate?: Date;
    endDate?: Date;
    teams?: Team[];
    workers?: Worker[];
}
