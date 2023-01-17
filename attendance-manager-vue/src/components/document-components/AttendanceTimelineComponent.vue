<template>
  <v-container>
    <v-card flat>
      <v-card-text>
        <v-row justify="center">
          <v-col cols="6" v-if="documentFiles.length > 0">
            <v-timeline width="50%">
              <v-timeline-item
                v-for="(item, index) in documentFiles"
                :key="item.documentFileId"
                :class="index % 2 == 0 ? 'text-right' : ''"
              >
                <v-btn text>
                  {{ item.activityTime }} - {{ item.courseType }}
                </v-btn></v-timeline-item
              >
            </v-timeline>
          </v-col>
          <div v-else>
            <h3 class="pa-9">There is not file available!</h3>
          </div>
        </v-row>
        <template>
          <v-row justify="center">
            <v-dialog v-model="addAttendanceDialog" persistent max-width="50%">
              <template v-slot:activator="{ on, attrs }">
                <v-btn color="primary" dark v-bind="attrs" v-on="on">
                  Add attendance
                </v-btn>
              </template>
              <AddAttendanceDialog
                :documentId="documentId"
                @close="oncloseAddAttendanceDialog"
                @save="oncloseAddAttendanceDialog"
              />
            </v-dialog>
          </v-row> </template></v-card-text></v-card
  ></v-container>
</template>

<script lang="ts">
import { DocumentFileViewModule } from "@/modules/document/document-file";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import AddAttendanceDialog from "../document-components/AddAttendanceDialog.vue";

export default Vue.extend({
  components: {
    AddAttendanceDialog,
  },
  data() {
    return {
      addAttendanceDialog: false,
    };
  },
  computed: {
    documentFiles(): DocumentFileViewModule[] {
      return storeHelper.documentStore.documentFiles;
    },
    documentId(): string{
      return storeHelper.documentStore.documentDetails.documentId;
    }
  },
  methods: {
    oncloseAddAttendanceDialog(): void {
      this.addAttendanceDialog = false;
    },
  },
});
</script>