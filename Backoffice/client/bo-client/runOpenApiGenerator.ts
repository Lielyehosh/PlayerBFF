import {environment} from "./src/environments/environment";
import {get} from "http";
import {createWriteStream} from "fs";

function getSwagger() {
  return new Promise<void> ((resolve, reject) => {
    const writer = createWriteStream("./swagger.json");
    const fullUri = environment.baseUrl + "/swagger/" + environment.apiVersion + "/swagger.json";
    console.log('get swagger from', fullUri);
    get(fullUri,
      //{rejectUnauthorized: false},
      (res) => {
        console.log('writing to file', './swagger.json');
        res.pipe(writer);
        writer.on("close", resolve);
      }).on("error", reject);
  });
}

function runGenerator(): Promise<void> {
  process.argv = ['node','ng-openapi-gen', '-i', 'swagger.json', '-o', 'src/app/api'];
  console.log('running generator', process.argv.join(' '));
  return require("ng-openapi-gen/lib/ng-openapi-gen.js").runNgOpenApiGen();
}

async function doAll() {
  // await getSwagger();
  await runGenerator();
  console.log('done');
}

doAll().catch((e) => {
  console.error(e);
  process.exit(1);
});
