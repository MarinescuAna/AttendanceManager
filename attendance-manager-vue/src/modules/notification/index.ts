import { NotificationPriority } from "@/shared/enums";

export interface NotificationViewModel{
    notificationId: number;
    message: string;
    createdOn: string;
    isRead: boolean;
    priority: NotificationPriority;
}