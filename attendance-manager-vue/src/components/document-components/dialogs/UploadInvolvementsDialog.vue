<template>
  <v-card>
    <v-card-title class="mb-5"
      ><b>Upload users' names/emails that were involved in this activity</b>
      <v-spacer></v-spacer>
      <v-btn @click="onClose(false, false)" icon>
        <v-icon>mdi-close</v-icon>
      </v-btn></v-card-title
    >
    <v-stepper v-model="currentStep" elevation="0" vertical>
      <v-stepper-step :complete="currentStep > 1" color="black" step="1">
        Import file
        <small>Import the excel or csv file</small>
      </v-stepper-step>
      <v-stepper-content step="1">
        <form enctype="multipart/form-data" novalidate>
          <v-file-input
            @change="onHandlerFile"
            label="File input"
            placeholder="Select your file"
            prepend-icon="mdi-paperclip"
            color="black"
            class="pa-5"
            accept=".xlsx,.csv"
            :show-size="1000"
            counter
            outlined
          >
            <template v-slot:selection="{ text }">
              {{ text }}
            </template>
          </v-file-input>
        </form>
      </v-stepper-content>

      <v-stepper-step :complete="currentStep > 2" step="2" color="black">
        Select sheet and the starting cell
      </v-stepper-step>
      <v-stepper-content step="2">
        <v-select
          class="px-2"
          color="black"
          :items="sheets"
          label="Available sheets"
          @change="onSelectSheet"
        ></v-select>
        <div v-if="sheets != null && sheets.length != 0">
          <v-layout wrap>
            <v-select
              color="black"
              :items="cells"
              v-model="startingCell"
              label="Select the starting cell"
              class="px-2"
              :disable="cells.length == 0"
            ></v-select>
            <v-select
              color="black"
              class="px-2"
              :items="cells"
              label="Select the ending cell"
              v-model="endingCell"
              :disable="cells.length == 0"
            ></v-select>
          </v-layout>
        </div>
        <v-layout row align-end column="4" class="my-3 mr-4">
          <v-flex>
            <v-btn class="dark_button white--text" @click="currentStep = 1">
              <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <v-btn
              class="dark_button white--text"
              @click="onMatchedData"
              :disabled="endingCell == '' || startingCell == ''"
            >
              <v-icon>mdi-arrow-right</v-icon>
            </v-btn>
          </v-flex>
        </v-layout>
      </v-stepper-content>

      <v-stepper-step :complete="currentStep > 3" step="3" color="black">
        Matched data
      </v-stepper-step>
      <v-stepper-content step="3">
        <!--Attendance table-->
        <MessageComponent
          description="No student matches your file. Please try again or select another group of cells."
          icon="mdi-alert"
          color="red"
          fontWeight="bold"
          v-if="matchedStudents.length == 0"
        />
        <v-simple-table class="ma-2">
          <template v-slot:default>
            <thead>
              <tr>
                <th class="text-left black--text text-h6">Name</th>
                <th class="text-left black--text text-h6">Email</th>
                <th class="text-left black--text text-h6">Attendance</th>
                <th class="text-left black--text text-h6">Bonus Points</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in matchedStudents" :key="item.involvementId">
                <td>{{ getName(item.userId) }}</td>
                <td>{{ item.userId }}</td>
                <td>
                  <v-checkbox
                    v-model="item.isPresent"
                    color="black"
                    @change="onPresenceChanged(item)"
                  ></v-checkbox>
                </td>
                <td>
                  <v-text-field
                    type="number"
                    color="black"
                    v-model="item.bonusPoints"
                    :disabled="!item.isPresent"
                  ></v-text-field>
                </td>
              </tr>
              <tr v-for="item in unmatchedStudents" :key="item.involvementId">
                <td>
                  <span class="red--text font-weight-bold">{{
                    getName(item.userId)
                  }}</span>
                </td>
                <td>
                  <span class="red--text font-weight-bold">{{
                    item.userId
                  }}</span>
                </td>
                <td>
                  <v-checkbox
                    v-model="item.isPresent"
                    color="black"
                    @change="onPresenceChanged(item)"
                  ></v-checkbox>
                </td>
                <td>
                  <v-text-field
                    color="black"
                    type="number"
                    v-model="item.bonusPoints"
                    :disabled="!item.isPresent"
                  ></v-text-field>
                </td>
              </tr>
            </tbody>
          </template>
        </v-simple-table>
        <v-layout row align-end column="4" class="my-3 mr-4">
          <v-flex>
            <v-btn class="dark_button white--text" @click="currentStep = 2">
              <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <v-btn color="success" @click="onClose(true, false)">
              Update involvements
            </v-btn>
            <v-btn color="red" @click="onClose(false, true)"> Discard </v-btn>
          </v-flex>
        </v-layout>
      </v-stepper-content>
    </v-stepper>
  </v-card>
</template>

<script lang="ts">
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import { UpdateInvolvementDto } from "@/modules/commands-parameters";
import { InvolvementViewModule } from "@/modules/view-modules";
import Vue from "vue";
import * as XLSX from "xlsx";

export default Vue.extend({
  name: "UploadInvolvementsDialog",
  components: { MessageComponent },
  props: {
    currentInvolvements: {
      type: Array as () => InvolvementViewModule[],
      required: true,
    },
  },
  data() {
    return {
      currentStep: 1,
      startingCell: "",
      endingCell: "",
      cells: [] as string[],
      workbook: {} as XLSX.WorkBook,
      selectedSheet: {} as XLSX.WorkSheet,
      unmatchedStudents: [] as UpdateInvolvementDto[],
      matchedStudents: [] as UpdateInvolvementDto[],
    };
  },
  computed: {
    sheets: function (): string[] {
      return this.workbook.SheetNames;
    },
  },
  methods: {
    getName: function (email: string): string {
      return this.currentInvolvements.find((e) => e.email == email)!.student;
    },
    onSelectSheet: function (sheet): void {
      this.selectedSheet = this.workbook.Sheets[sheet];
      const pattern = /^[A-Za-z]{1,3}\d+$/;
      for (var cell in this.selectedSheet) {
        if (pattern.test(cell)) {
          this.cells.push(cell);
        }
      }
      this.cells.sort();
    },
    onMatchedData: function (): void {
      this.currentStep = 3;
      const range = `${this.startingCell}:${this.endingCell}`;
      const selectedCells = XLSX.utils.sheet_to_json(this.selectedSheet, {
        raw: true,
        range: range,
        header: "A",
      });

      for (let element of this.currentInvolvements) {
        const student = selectedCells.find(
          (e) =>
            (e as { A: string }).A.toLowerCase() ==
              element.email.toLowerCase() ||
            (e as { A: string }).A.toLowerCase() ==
              element.student.toLowerCase()
        );
        if (student != undefined) {
          this.matchedStudents.push({
            bonusPoints: 0,
            involvementId: element.involvementId,
            collectionId: 0,
            isPresent: true,
            userId: element.email,
          });
        } else {
          this.unmatchedStudents.push({
            bonusPoints: 0,
            involvementId: element.involvementId,
            collectionId: 0,
            isPresent: false,
            userId: element.email,
          });
        }
      }
    },
    onHandlerFile: function (event): void {
      let fileReader = new FileReader();
      fileReader.readAsArrayBuffer(event);
      fileReader.onload = () => {
        let arrayBuffer: ArrayBuffer | string | null;
        arrayBuffer = fileReader.result as ArrayBuffer;
        var data = new Uint8Array(arrayBuffer);
        var arr: string[] = [];
        for (var i = 0; i != data.length; ++i)
          arr[i] = String.fromCharCode(data[i]);
        var bstr = arr.join("");
        this.workbook = XLSX.read(bstr, { type: "binary" });
      };

      this.currentStep = 2;
    },
    onClose: function (saveDate: boolean, discard: boolean): void {
      if (discard) {
        if (confirm("Are you sure that you what to discard the imports?")) {
          this.$emit("close");
        }
      }
      if (!discard && !saveDate) {
        this.$emit("close");
      }
      if (!discard && saveDate) {
        if (
          confirm(
            "Are you sure that you what to update the current involvements? The old data will be overwritten."
          )
        ) {
          this.$emit(
            "update",
            this.matchedStudents.concat(this.unmatchedStudents)
          );
        }
      }
    },
    /**If the student is not present, then the bonus points cannot be inserted */
    onPresenceChanged: function (item: UpdateInvolvementDto): void {
      if (!item.isPresent) {
        item.bonusPoints = 0;
      }
    },
  },
});
</script>