<template>
  <v-container>
    <v-layout justify-center class="pa-2">
      <v-flex md5 lg5 v-if="documentFiles.length > 0">
        <div v-if="!isMobile">
          <v-timeline>
            <v-timeline-item
              v-for="(item, index) in documentFiles"
              :key="item.attendanceCollectionId"
              :class="index % 2 == 0 ? 'text-right' : ''"
              color="black"
            >
              <v-btn
                text
                @click="onOpenAttendanceDialog(item.attendanceCollectionId, item.activityTime)"
              >
                {{ item.activityTime.replaceAll('/','.') }} - {{ item.courseType }}
              </v-btn>
            </v-timeline-item>
          </v-timeline>
        </div>
        <div v-else>
          <!-- diplay the bullets as a list when the user is on mobile-->
          <v-list dense>
            <v-list-item
              v-for="(item, index) in documentFiles"
              :key="item.attendanceCollectionId"
              @click="onOpenAttendanceDialog(item.attendanceCollectionId, item.activityTime)"
            >
              <v-list-item-content>
                <v-divider v-if="index != 0" class="mb-2"></v-divider>
                <v-list-item-title
                  >{{ item.activityTime }} -
                  {{ item.courseType }}</v-list-item-title
                >
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </div>
      </v-flex>
      <v-flex v-else>
        <h3 class="pa-9">There is not collection available!</h3>
      </v-flex>
    </v-layout>
    <v-layout justify-center>
      <v-btn color="black" dark @click="addAttendanceDateDialog = true" v-if="isTeacher">
        Add attendance
      </v-btn></v-layout
    >

    <!--Dialog for displaing the details about an attendance collection-->
    <v-dialog
      v-if="addAttendanceDialog"
      v-model="addAttendanceDialog"
      fullscreen
      hide-overlay
      scrollable
    >
      <UpdateInvolvementsDialog
        :attendanceCollectionId="selectedCollectionId"
        :attendanceCollectionDate="selectedCollectionDate"
        @close-dialog="addAttendanceDialog = false"
      />
    </v-dialog>

    <!--Dialog for displaing the add new attendance form-->
    <v-dialog
      v-if="addAttendanceDateDialog"
      v-model="addAttendanceDateDialog"
      persistent
      max-width="50%"
      :fullscreen="isMobile"
    >
      <AddAttendanceDateDialog
        @close="oncloseAddAttendanceDateDialog"
        @save="oncloseAddAttendanceDateDialog"
      />
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import AddAttendanceDateDialog from "@/components/document-components/dialogs/AddAttendanceDateDialog.vue";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import UpdateInvolvementsDialog from "@/components/document-components/dialogs/UpdateInvolvementsDialog.vue";

export default Vue.extend({
  name: "AttendanceTimelineComponent",
  components: {
    AddAttendanceDateDialog,
    UpdateInvolvementsDialog
},
  data() {
    return {
      // Display dialog for adding a new timeline
      addAttendanceDateDialog: false,
      // Display dialog for adding attendances
      addAttendanceDialog: false,
      // CollectionId which will be open in the dialog
      selectedCollectionId: 0,
      /** Collection date */
      selectedCollectionDate: ''
    };
  },
  computed: {
    documentFiles: function (): AttendanceCollectionViewModule[] {
      return storeHelper.documentStore.documentFiles;
    },
    documentId: function (): number {
      return storeHelper.documentStore.documentDetails.documentId;
    },
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
  },
  methods: {
    oncloseAddAttendanceDateDialog: function (): void {
      this.addAttendanceDateDialog = false;
    },
    onOpenAttendanceDialog: function (collectionId: number, date: string): void {
      this.addAttendanceDialog = true;
      this.selectedCollectionDate = date;
      this.selectedCollectionId = collectionId;
    },
  },
});
</script>