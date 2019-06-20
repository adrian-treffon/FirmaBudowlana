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
import { NavComponent } from './nav/nav.component';
import { NavResolver } from './resolvers/nav.resolver';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderDetailsResolver } from './resolvers/order-details.resolver';
import { OrdersAllListComponent } from './orders-all-list/orders-all-list.component';
import { OrdersAllResolver } from './resolvers/orders-all-list.resolver';
import { PaymentPaidListComponent } from './payment-paid-list/payment-paid-list.component';
import { PaymentUnpaidListComponent } from './payment-unpaid-list/payment-unpaid-list.component';
import { EditWorkerComponent } from './edit-worker/edit-worker.component';
import { WorkerEditResolver } from './resolvers/worker-edit.resolver';
import { EditTeamComponent } from './edit-team/edit-team.component';
import { EditTeamResolver } from './resolvers/edit-team.resolver';
import { EditValidatedOrderComponent } from './edit-validated-order/edit-validated-order.component';
import { ReportInputComponent } from './report-input/report-input.component';
import { ReportInputResolver } from './resolvers/report-input.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    { path: 'nav', component: NavComponent, resolve: {token: NavResolver}},
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
            { path: 'payment-paid-list', component: PaymentPaidListComponent},
            { path: 'report-input', component: ReportInputComponent, resolve: {result: ReportInputResolver}},
            { path: 'payment-unpaid-list', component: PaymentUnpaidListComponent},
            { path: 'order-all-list', component: OrdersAllListComponent, resolve: {orders: OrdersAllResolver}},
            { path: 'new-team', component: NewTeamComponent, resolve: {workers: NewTeamResolver}},
            { path: 'validate-order/:id', component: ValidateOrderComponent, pathMatch: 'full', resolve: {teams: ValidateOrderResolver}},
            { path: 'order-details/:id', component: OrderDetailsComponent, pathMatch: 'full', resolve: {order: OrderDetailsResolver}},
            { path: 'edit-worker/:id', component: EditWorkerComponent, pathMatch: 'full', resolve: {worker: WorkerEditResolver}},
            { path: 'edit-team', component: EditTeamComponent, resolve: {workers: EditTeamResolver}},
            { path: 'edit-validated-order', component: EditValidatedOrderComponent, pathMatch: 'full',
                    resolve: {teams: ValidateOrderResolver}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'}
];
