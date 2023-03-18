<template>
    <div>
        <v-layout column>
            <v-flex class="mx-md-5 my-1">
                <v-autocomplete v-model="search"
                                :items="fullnames"
                                persistent-hint
                                label="Search (name)"
                                maxlength="128"
                                color="black"
                                append-icon="mdi-magnify">
                </v-autocomplete>
            </v-flex>
            <v-flex class="mx-md-5 mb-2">
                <v-data-table :headers="totalAttendancesHeader"
                              :items="attendances"
                              :search="search"
                              :expanded.sync="expanded"
                              show-expand
                              single-expand
                              item-key="userID"
                              dense
                              class="elevation-1">
                    <template v-slot:expanded-item="{ headers, item }">
                        <td :colspan="headers.length">
                            <h1 class="fond-weight-bold ma-3">{{ item.userName }}</h1>
                            <StudentAttendanceExpandedComponent :userId="item.userID" />
                        </td>
                    </template>
                </v-data-table>
            </v-flex>
        </v-layout>
    </div>
</template>

<script lang="ts">
    import Vue from "vue";
    import storeHelper from "@/store/store-helper";
    import { totalAttendancesHeader } from "@/components/document-components/TotalAttendancesHeader";
    import { TotalAttendanceModule } from "@/modules/document/attendance";
    import AttendanceService from "@/services/attendance.service";
    import StudentAttendanceExpandedComponent from "@/components/document-components/StudentAttendanceExpandedComponent.vue";

    export default Vue.extend({
        components: {
            StudentAttendanceExpandedComponent,
        },
        data() {
            return {
                search: "",
                totalAttendancesHeader,
                attendances: [] as TotalAttendanceModule[],
                // Elements that are currently expanded
                expanded: [],
            };
        },
        computed: {
            fullnames: function () {
                return this.attendances.map((x) => x.userName);
            },
        },
        created: async function () {
            this.attendances = await AttendanceService.getTotalAttendancesByDocumentId(
                storeHelper.documentStore.documentDetails.documentId
            );
        },
    });
</script>