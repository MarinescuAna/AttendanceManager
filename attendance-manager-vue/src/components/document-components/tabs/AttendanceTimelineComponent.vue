<template>
    <v-container>
        <v-card flat>
            <v-card-text>
                <v-row justify="center">
                    <v-col cols="6" v-if="documentFiles.length > 0">
                        <v-timeline width="50%">
                            <v-timeline-item v-for="(item, index) in documentFiles"
                                             :key="item.attendanceCollectionId"
                                             :class="index % 2 == 0 ? 'text-right' : ''"
                                             color="black">
                                <v-btn text
                                       @click="onOpenAttendanceDialog(item.attendanceCollectionId)">
                                    {{ item.activityTime }} - {{ item.courseType }}
                                </v-btn>
                            </v-timeline-item>
                        </v-timeline>

                        <v-dialog v-if="addAttendanceDialog"
                                  v-model="addAttendanceDialog"
                                  fullscreen
                                  hide-overlay
                                  scrollable>
                            <AddAttendanceDialog :attendanceCollectionId="collectionId"
                                                 @close-dialog="addAttendanceDialog = false" />
                        </v-dialog>
                    </v-col>
                    <div v-else>
                        <h3 class="pa-9">There is not file available!</h3>
                    </div>
                </v-row>
                <v-row justify="center" class="mt-2">
                    <v-btn color="black" dark @click="addAttendanceDateDialog = true">
                        Add attendance
                    </v-btn>
                    <v-dialog v-if="addAttendanceDateDialog"
                              v-model="addAttendanceDateDialog"
                              persistent
                              max-width="50%">
                        <AddAttendanceDateDialog @close="oncloseaddAttendanceDateDialog"
                                                 @save="oncloseaddAttendanceDateDialog" />
                    </v-dialog>
                </v-row>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script lang="ts">
    import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
    import storeHelper from "@/store/store-helper";
    import Vue from "vue";
    import AddAttendanceDateDialog from "@/components/document-components/dialogs/AddAttendanceDateDialog.vue";
    import AddAttendanceDialog from "@/components/document-components/dialogs/AddAttendanceDialog.vue";

    export default Vue.extend({
        components: {
            AddAttendanceDialog,
            AddAttendanceDateDialog,
        },
        data() {
            return {
                // Display dialog for adding a new timeline
                addAttendanceDateDialog: false,
                // Display dialog for adding attendances
                addAttendanceDialog: false,
                // CollectionId which is passed to the dialog
                collectionId: 0,
            };
        },
        computed: {
            documentFiles: function (): AttendanceCollectionViewModule[] {
                return storeHelper.documentStore.documentFiles;
            },
            documentId: function (): number {
                return storeHelper.documentStore.documentDetails.documentId;
            },
        },
        methods: {
            oncloseaddAttendanceDateDialog: function (): void {
                this.addAttendanceDateDialog = false;
            },

            onOpenAttendanceDialog: function (collectionId: number): void {
                this.addAttendanceDialog = true;
                this.collectionId = collectionId;
            },
        },
    });
</script>