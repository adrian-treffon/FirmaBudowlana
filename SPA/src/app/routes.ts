import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { OrdersComponent } from './orders/orders.component';
import { NewOrderComponent } from './newOrder/newOrder.component';
import { AdminMenuComponent } from './admin-menu/admin-menu.component';
import { WorkerListComponent } from './worker-list/worker-list.component';
import { NewWorkerComponent } from './new-worker/new-worker.component';
import { TeamListComponent } from './team-list/team-list.component';
import { OrderListComponent } from './order-list/order-list.component';
import { NewTeamComponent } from './new-team/new-team.component';
import { NewTeamResolver } from './resolvers/new-team.resolver';
import { ValidateOrderComponent } from './validate-order/validate-order.component';
import { ValidateOrderResolver } from './resolvers/validate-order.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'orders', component: OrdersComponent},
            { path: 'newOrder', component: NewOrderComponent },
            { path: 'admin-menu', component: AdminMenuComponent },
            { path: 'worker-list', component: WorkerListComponent},
            { path: 'new-worker', component: NewWorkerComponent },
            { path: 'team-list', component: TeamListComponent },
            { path: 'order-list', component: OrderListComponent},
            { path: 'new-team', component: NewTeamComponent, resolve: {workers: NewTeamResolver}},
            { path: 'validate-order', component: ValidateOrderComponent, resolve: {teams: ValidateOrderResolver}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'}
];
