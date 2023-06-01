import ResponseHandler from "@/error-handler/error-handler";
import { NotificationViewModel } from "@/modules/notification";
import https from "@/plugins/axios";
import { NOTIFICATION_CONTROLLER } from "@/shared/constants";

export default class NotificationService {
    static async getCurrentUserNotificationsAsync(): Promise<NotificationViewModel[]> {
        return (await https.get(`${NOTIFICATION_CONTROLLER}/notifications`)).data;
    }
    static async removeMessageAsync(notificationId: number): Promise<boolean> {
        let isSuccess = true;
        await https.delete(`${NOTIFICATION_CONTROLLER}/delete_notification?id=${notificationId}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        return isSuccess;
    }
    static async readMessageAsync(notificationId: number): Promise<boolean> {
        let isSuccess = true;
        await https.patch(`${NOTIFICATION_CONTROLLER}/read_message?id=${notificationId}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        return isSuccess;
    }
}