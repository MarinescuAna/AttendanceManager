
import https from "@/plugins/axios";
import { ATTENDANCE_CONTROLLER } from "@/shared/constants";
import ResponseHandler from "@/error-handler/error-handler";
import { UpdateInvolvementDto, UpdateInvolvementsParameters, UpdateInvolvementByCodeParameters } from "@/modules/commands-parameters";
import { InvolvementViewModule, TotalInvolvementViewModule } from "@/modules/view-modules";

export default class InvolvementService {

    /**Use this method to can get the new involvements update */
    static getInvolvementChanges(oldInvolvementsList: InvolvementViewModule[], newInvolvementList: InvolvementViewModule[]): UpdateInvolvementDto[] {
        const results: UpdateInvolvementDto[] = [];

        oldInvolvementsList.forEach((involvement) => {
            const currentInvolvement = newInvolvementList.find(
                (x) => x.involvementId === involvement.involvementId
            );
            if ((currentInvolvement != null || currentInvolvement != undefined) &&
                currentInvolvement?.bonusPoints !== involvement.bonusPoints ||
                currentInvolvement?.isPresent !== involvement.isPresent
            ) {
                results.push({
                    involvementId: currentInvolvement?.involvementId,
                    bonusPoints: currentInvolvement?.bonusPoints,
                    isPresent: currentInvolvement?.isPresent,
                    userId: currentInvolvement?.email,
                    collectionId: currentInvolvement?.collectionId
                } as UpdateInvolvementDto);
            }
        });

        return results;
    }

    static isChanged(involvementsInit: InvolvementViewModule[], involvements: InvolvementViewModule[]): boolean {
        for (let i = 0; i < involvements.length; i++) {
            if (
                involvements[i].bonusPoints !=
                involvementsInit[i].bonusPoints ||
                involvements[i].isPresent != involvementsInit[i].isPresent
            ) {
                return true;
            }
        }
        return false;
    }

    static async getInvolvementsTotal(): Promise<TotalInvolvementViewModule[]> {
        return (await https.get(`${ATTENDANCE_CONTROLLER}/total_involvements`)).data;
    }

    static async getInvolvements(collectionId: number, email: string, useCode: boolean, currentUser: boolean, onlyPresents: boolean): Promise<InvolvementViewModule[]> {
        return (await https.get(`${ATTENDANCE_CONTROLLER}/involvements?email=${email}&collection_id=${collectionId}&use_code=${useCode}&current_user=${currentUser}&only_present=${onlyPresents}`)).data;
    }

    static async addStudentsAttendances(payload: UpdateInvolvementsParameters): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${ATTENDANCE_CONTROLLER}/update_student_involvement`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        return isSuccess;
    }

    /**
     * Update the students attendances by code and attendance id
     */
    static async updateAttendanceByCode(payload: UpdateInvolvementByCodeParameters): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${ATTENDANCE_CONTROLLER}/update_involvement_by_code`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        return isSuccess;
    }
}