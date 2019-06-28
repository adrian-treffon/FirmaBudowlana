export interface Payment {
    paymentID: string;
    workerID: string;
    orderID: string;
    paymentDate: Date;
    amount: number;
}
