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
                @click="onOpenAttendanceDialog(item.attendanceCollectionId)"
              >
                {{ item.activityTime }} - {{ item.courseType }}
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
              @click="onOpenAttendanceDialog(item.attendanceCollectionId)"
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
        <h3 class="pa-9">There is not file available!</h3>
      </v-flex>
    </v-layout>
    <v-layout justify-center>
      <v-btn color="black" dark @click="addAttendanceDateDialog = true">
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
      <AddAttendanceDialog
        :attendanceCollectionId="collectionId"
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
        @close="oncloseaddAttendanceDateDialog"
        @save="oncloseaddAttendanceDateDialog"
      />
    </v-dialog>
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
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
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