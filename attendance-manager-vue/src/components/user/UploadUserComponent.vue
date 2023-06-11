<template>
  <v-layout class="orange lighten-3" column>
    <v-stepper v-model="currentStep" vertical>
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
        Match columns with data about users
      </v-stepper-step>
      <v-stepper-content step="2">
        <label>The role of uploaded users</label>
        <v-radio-group v-model="role" class="ma-4">
          <v-radio label="Student" :value="1"></v-radio>
          <v-radio label="Teacher" :value="2"></v-radio>
        </v-radio-group>
        <v-layout v-if="role != 0" column>
          <v-layout wrap>
            <label class="my-3">Select the sheet that contains the data</label>
            <v-select
              class="px-2 custom-select-sheet"
              color="black"
              :items="sheets"
              @change="onSelectSheet"
              outlined
              searchable
            ></v-select>
          </v-layout>
          <v-layout wrap>
            <label class="my-3"
              >Select the column/row which contains the users name</label
            >
            <v-select
              color="black"
              :items="cells"
              v-model="fullnameStartCell"
              label="Start cell"
              class="px-2 custom-select-cells"
              outlined
              searchable
            ></v-select
            ><span class="colon-custom">:</span>
            <v-select
              color="black"
              class="px-2 custom-select-cells"
              :items="cells"
              label="End cell"
              v-model="fullnameEndCell"
              outlined
              searchable
            ></v-select>
          </v-layout>
          <v-layout wrap>
            <label class="my-3"
              >Select the column/row which contains the users email</label
            >
            <v-select
              color="black"
              :items="cells"
              v-model="emailStartCell"
              label="Start cell"
              class="px-2 custom-select-cells"
              outlined
            ></v-select
            ><span class="colon-custom">:</span>
            <v-select
              color="black"
              class="px-2 custom-select-cells"
              :items="cells"
              label="End cell"
              v-model="emailEndCell"
              outlined
            ></v-select>
          </v-layout>
          <v-layout wrap>
            <label class="my-3"
              >Select the column/row which contains the users GDPR code</label
            >
            <v-select
              color="black"
              :items="cells"
              v-model="gdprStartCell"
              label="Start cell"
              class="px-2 custom-select-cells"
              outlined
            ></v-select
            ><span class="colon-custom">:</span>
            <v-select
              color="black"
              class="px-2 custom-select-cells"
              :items="cells"
              label="End cell"
              v-model="gdprEndCell"
              outlined
            ></v-select>
          </v-layout>
          <v-layout wrap>
            <label class="my-3">{{
              role == 1 ? "Enrollment year" : "Employment year"
            }}</label>
            <v-select
              :items="years"
              v-model="year"
              class="px-2 custom-select-sheet"
              color="black"
              outlined
            ></v-select>
          </v-layout>
          <v-layout wrap>
            <label class="my-3">Select department</label>
            <v-select
              :items="departments"
              @change="onFillSpecializations"
              class="px-2 custom-select-section"
              color="black"
              item-text="name"
              item-value="id"
              ref="departmentRef"
              outlined
            ></v-select>
          </v-layout>
          <v-layout wrap>
            <label class="my-3">Select specialization(s)</label>
            <v-select
              :items="specializations"
              v-model="selectedSpecializations"
              class="px-2 custom-select-section"
              color="black"
              item-text="name"
              item-value="id"
              :disabled="specializations.length == 0"
              chips
              deletable-chips
              multiple
              outlined
              v-if="role == 2"
            ></v-select>
            <v-select
              :items="specializations"
              v-model="selectedSpecialization"
              class="px-2 custom-select-section"
              color="black"
              item-text="name"
              item-value="id"
              :disabled="specializations.length == 0"
              outlined
              v-else
            ></v-select>
          </v-layout>
        </v-layout>
        <v-layout row align-end column="4" class="my-3 mr-4">
          <v-flex>
            <v-btn class="white--text" color="black" @click="currentStep = 1">
              <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <v-btn class="white--text" @click="onGenerateTable" color="black">
              <v-icon>mdi-arrow-right</v-icon>
            </v-btn>
          </v-flex>
        </v-layout>
      </v-stepper-content>

      <v-stepper-step :complete="currentStep > 3" step="3" color="black">
        Final result
      </v-stepper-step>
      <v-stepper-content step="3">
        <v-btn @click="newRow = true" class="white--text" color="black"
          >Add new raw</v-btn
        >
        <validation-observer ref="observer" v-slot="{ handleSubmit }">
          <v-form @submit.prevent="handleSubmit(onSaveUsers)">
            <v-simple-table class="ma-2">
              <template v-slot:default>
                <thead>
                  <tr>
                    <th class="text-left black--text text-h6">Fullname</th>
                    <th class="text-left black--text text-h6">Email</th>
                    <th class="text-left black--text text-h6">GDPR</th>
                    <th class="text-left black--text text-h6">Role</th>
                    <th class="text-left black--text text-h6">
                      Specialization
                    </th>
                    <th class="text-left black--text text-h6">
                      {{ role == 1 ? "Enrollment year" : "Employment year" }}
                    </th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in users" :key="item.code">
                    <td>
                      <validation-provider
                        :rules="rules.required"
                        name="fullname"
                        v-slot="{ errors }"
                      >
                        <v-text-field
                          v-model="item.fullname"
                          color="black"
                          :error-messages="errors"
                        ></v-text-field
                      ></validation-provider>
                    </td>
                    <td>
                      <validation-provider
                        :rules="rules.email"
                        name="email"
                        v-slot="{ errors }"
                      >
                        <v-text-field
                          v-model="item.email"
                          color="black"
                          :error-messages="errors"
                        ></v-text-field>
                      </validation-provider>
                    </td>
                    <td class="custom-column-code">
                      <v-text-field
                        v-model="item.code"
                        color="black"
                      ></v-text-field>
                    </td>
                    <td class="custom-column-size">
                      {{ role == 1 ? "Student" : "Teacher" }}
                    </td>
                    <td>
                      <v-chip
                        v-for="spec in item.specializationIds"
                        :key="spec"
                        >{{ getSpecializationName(spec) }}</v-chip
                      >
                    </td>
                    <td class="custom-column-size">{{ item.year }}</td>
                    <th>
                      <v-btn
                        @click="onDeleteItem(item.code)"
                        :disabled="item.code == ''"
                        icon
                        ><v-icon>mdi-delete</v-icon></v-btn
                      >
                    </th>
                  </tr>
                  <tr v-if="newRow">
                    <td>
                      <v-text-field
                        v-model="newName"
                        color="black"
                      ></v-text-field>
                    </td>
                    <td>
                      <v-text-field
                        v-model="newEmail"
                        class=""
                        color="black"
                      ></v-text-field>
                    </td>
                    <td class="custom-column-code">
                      <v-text-field
                        v-model="newCode"
                        color="black"
                      ></v-text-field>
                    </td>
                    <td class="custom-column-size">
                      {{ role == 1 ? "Student" : "Teacher" }}
                    </td>
                    <td>
                      <v-chip v-if="role == 1">{{
                        getSpecializationName(selectedSpecialization)
                      }}</v-chip>
                      <div v-else>
                        <v-chip
                          v-for="spec in selectedSpecializations"
                          :key="spec"
                          >{{ getSpecializationName(spec) }}</v-chip
                        >
                      </div>
                    </td>
                    <td class="custom-column-size">{{ year }}</td>
                    <th>
                      <v-layout>
                        <v-btn @click="onDeleteNewItem" icon
                          ><v-icon>mdi-delete</v-icon></v-btn
                        >
                        <v-btn
                          @click="onSaveNewRow"
                          :disabled="
                            newCode == '' || newEmail == '' || newName == ''
                          "
                          icon
                          ><v-icon>mdi-plus-thick</v-icon></v-btn
                        ></v-layout
                      >
                    </th>
                  </tr>
                </tbody>
              </template>
            </v-simple-table>
            <v-layout column="4" class="my-3 mr-4" row align-end>
              <v-flex>
                <v-btn
                  class="white--text mx-1"
                  color="black"
                  @click="currentStep = 2"
                >
                  <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                <v-btn @click="onSaveUsers" class="mx-1" color="success">
                  Save
                </v-btn>
                <v-btn class="mx-1" color="red"> Discard </v-btn>
              </v-flex>
            </v-layout>
          </v-form>
        </validation-observer>
      </v-stepper-content>
    </v-stepper>
  </v-layout>
</template>

<style scoped>
.colon-custom {
  font-size: 37px;
  font-weight: bold;
}
.custom-select-cells {
  width: 150px;
  flex: unset;
}
.custom-select-sheet {
  width: 300px;
  flex: unset;
}
.custom-select-section {
  width: 500px;
  flex: unset;
}
.custom-column-code {
  width: 150px;
}
.custom-column-size {
  width: 50px;
}
</style>

<script lang="ts">
import { Role } from "@/shared/enums";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import * as XLSX from "xlsx";
import { rules } from "@/plugins/vee-validate";
import { DepartmentViewModule, SpecializationViewModule } from "@/modules/view-modules";
import { InsertUserParameters } from "@/modules/commands-parameters";

export default Vue.extend({
  name: "UploadUserComponent",
  data: function () {
    return {
      rules,
      currentStep: 1,
      workbook: {} as XLSX.WorkBook,
      role: 0,
      selectedSheet: {} as XLSX.WorkSheet,
      cells: [] as string[],
      fullnameStartCell: "",
      fullnameEndCell: "",
      emailStartCell: "",
      emailEndCell: "",
      gdprStartCell: "",
      gdprEndCell: "",
      year: 0,
      specializations: [] as SpecializationViewModule[],
      // Selected specializations
      selectedSpecializations: [] as number[],
      // Selected specialization if just one specialization can be selected
      selectedSpecialization: 0,
      users: [] as InsertUserParameters[],
      newRow: false,
      newName: "",
      newCode: "",
      newEmail: "",
    };
  },
  computed: {
    sheets: function (): string[] {
      return this.workbook.SheetNames;
    },
    /**
     * Get all the years between the current year and 1950
     */
    years(): string[] {
      return Array.from(Array(new Date().getFullYear() - 1949), (_, i) =>
        (new Date().getFullYear() - i).toString()
      );
    },
    /**
     * All departments
     */
    departments(): DepartmentViewModule[] {
      return storeHelper.departmentStore.departments;
    },
  },
  methods: {
    onSaveUsers: function (): void {
        this.$nextTick(() => {
        this.$refs.observer.$emit("validate");
      });
    },
    onDeleteNewItem: function (): void {
      this.newRow = false;
      this.newCode = "";
      this.newEmail = "";
      this.newName = "";
    },
    onSaveNewRow: function (): void {
      this.users.push({
        code: this.newCode,
        email: this.newEmail,
        fullname: this.newName,
        role: this.role.toString(),
        specializationIds:
          this.role == 1
            ? [this.selectedSpecialization]
            : this.selectedSpecializations,
        year: this.year,
      });
      this.onDeleteNewItem();
    },
    onDeleteItem: function (code: string): void {
      this.users = this.users.filter((u) => u.code != code);
    },
    getSpecializationName: function (id: number): string {
      return this.specializations.find((s) => s.id == id)!.name;
    },
    onGenerateTable: function (): void {
      this.users = [];
      const names = this._getStringsFromRange(
        this.fullnameStartCell.match(/[A-Za-z]+/)![0],
        `${this.fullnameStartCell}:${this.fullnameEndCell}`
      );
      const emails = this._getStringsFromRange(
        this.emailStartCell.match(/[A-Za-z]+/)![0],
        `${this.emailStartCell}:${this.emailEndCell}`
      );
      const codes = this._getStringsFromRange(
        this.gdprStartCell.match(/[A-Za-z]+/)![0],
        `${this.gdprStartCell}:${this.gdprEndCell}`
      );

      let specializationIds: number[] = [];
      if (this.role == Role.Teacher) {
        specializationIds = this.selectedSpecializations;
      } else {
        specializationIds.push(this.selectedSpecialization);
      }

      for (let i = 0; i < names.length; i++) {
        this.users.push({
          code: codes[i],
          email: emails[i],
          fullname: names[i],
          role: this.role.toString(),
          specializationIds: specializationIds,
          year: this.year,
        });
      }
      this.currentStep = 3;
    },
    _getStringsFromRange: function (letter: string, range: string): string[] {
      const selectedrange = XLSX.utils.sheet_to_json(this.selectedSheet, {
        raw: true,
        header: [letter],
        range: range,
      });
      return selectedrange.map((element) => (element as object)[letter]);
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
    /**
     * Get the list with all specializations by department id
     * @param selectedDepartment
     */
    onFillSpecializations(selectedDepartment: number): void {
      this.specializations =
        storeHelper.specializationStore.specializations.filter(
          (specialization) => specialization.departmentId == selectedDepartment
        );
      this.selectedSpecializations = [];
    },
    /**
     * Reset specialization v-selector when the role is changed
     */
    onResetSpecializationSelector(): void {
      this.selectedSpecializations = [];
    },
  },
});
</script>