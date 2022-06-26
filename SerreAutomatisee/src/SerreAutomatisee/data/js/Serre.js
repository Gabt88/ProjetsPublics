let controleAutomatiqueActive;
let temperatureInterieure = document.getElementById("temperatureInterieure");
let temperatureExterieure = document.getElementById("temperatureExterieure");
let pourcentageOuverture = document.getElementById("pourcentageOuverture");
let textePourcentageOuverture = document.getElementById("ouverture");
let temperatureDesireeTexte = document.getElementById("temperatureDesireeTexte");
let pourcentageOuvertureDesireeTexte = document.getElementById("angleDesireeTexte");
const toitOuvert = 100;
const toitFerme = 0;

// mock test
// let tempInterieure = 30;
// let tempExterieure = 22;
// let pourOuverture = 25;
// let pourOuvertureDesiree = 25;
// let tempDesiree = 26;
// let mode = false;

document.getElementById("changerMode").addEventListener("click", modifierMode);
document.getElementById("changerTemperature").addEventListener("click", modifierTemperatureDesiree);

let changerOuverture = document.getElementsByClassName("changerOuverture");
Array.from(changerOuverture).forEach(element => 
	element.addEventListener("click", modifierOuvertureDesiree)
);

getInformationsSerre();
setInterval(getInformationsSerre, 2000);

function getInformationsSerre() 
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = () => 
	{
		if (xhttp.readyState == 4) 
		{
			let reponse = JSON.parse(xhttp.responseText);
			if (xhttp.status == 200)
			{
				controleAutomatiqueActive = reponse.configuration.controleAutomatique;
				miseAJourMode(controleAutomatiqueActive);
				miseAJourTemperatures(reponse.configuration.temperatureInterieure, reponse.configuration.temperatureExterieure);
				miseAJourPourcentageOuvertureDesiree(reponse.configuration.pourcentageOuvertureToitModeManuel);
				miseAJourTemperatureDesiree(reponse.configuration.temperatureInterieureDesiree);
				
				if(controleAutomatiqueActive)
				{
					miseAJourPourcentageOuverture(reponse.configuration.pourcentageOuvertureToitModeAutomatique);
				}
				else
				{
					miseAJourPourcentageOuverture(reponse.configuration.pourcentageOuvertureToitModeManuel);
				}
			}
			else 
			{
				console.log("erreur!");
			}
		}
	}

	xhttp.open("GET", "/serre", true);
	xhttp.send();

	// mock test
	// miseAJourTemperatures(tempExterieure, tempInterieure);
	// miseAJourPourcentageOuverture(pourOuverture);
	// miseAJourPourcentageOuvertureDesiree(pourOuvertureDesiree);
	// miseAJourTemperatureDesiree(tempDesiree);
	// miseAJourMode(mode);
	// controleAutomatiqueActive = mode;
}

function miseAJourTemperatures(p_temperatureInterieure, p_temperatureExterieure)
{
	temperatureInterieure.textContent = p_temperatureInterieure.toFixed(1) + "°C";
	temperatureExterieure.textContent = p_temperatureExterieure.toFixed(1) + "°C";
}

function miseAJourPourcentageOuverture(p_pourcentageOuverture)
{
	console.log(p_pourcentageOuverture);
	textePourcentageOuverture.textContent = "Ouverture toît (" + p_pourcentageOuverture +  "%): ";
	pourcentageOuverture.value = p_pourcentageOuverture.toString();
}

function miseAJourTemperatureDesiree(p_temperatureDesiree)
{
	temperatureDesireeTexte.textContent = "Température désirée (" + p_temperatureDesiree + "): ";
}

function miseAJourPourcentageOuvertureDesiree(p_ouvertureDesiree)
{
	pourcentageOuvertureDesireeTexte.textContent = "Pourcentage ouverture toît désiré (" + p_ouvertureDesiree + "): ";
}


function modifierTemperatureDesiree()
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = () => 
	{
		if (xhttp.readyState == 4) 
		{
			if (xhttp.status != 200) 
			{
				console.log("erreur!");
			}
			
			else
			{
				console.log("ok!");
			}
		}
	}
	
	let corpsRequete = '{"temperatureInterieureDesiree":' + document.getElementById("temperatureDesiree").value + '}';
	xhttp.open("PUT", "/modifTempDesiree", true);
	xhttp.send(corpsRequete);

	// mock test
	// tempDesiree = document.getElementById("temperatureDesiree").value;
}

function modifierOuvertureDesiree(e)
{
	let id = e.target.id;
	let pourcentageOuverture;

	if(id == "ouvrir")
	{
		pourcentageOuverture = 100;
	}

	else if(id == "fermer")
	{
		pourcentageOuverture = 0;
	}
	else
	{
		pourcentageOuverture = document.getElementById("angleDesiree").value;
	}

	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = () => 
	{
		if (xhttp.readyState == 4) 
		{
			if (xhttp.status != 200) 
			{
				console.log("erreur!");
			}
			
			else
			{
				console.log("ok!");
			}
		}
	}

	let corpsRequete = '{"pourcentageOuvertureToitModeManuel":' + pourcentageOuverture + '}';
	xhttp.open("PUT", "/modifPourcentageOuvertureManuel", true);
	xhttp.send(corpsRequete);

	// mock test
	// pourOuvertureDesiree = pourcentageOuverture;
}

function envoyerModificationMode(p_bool)
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = () => 
	{
		if (xhttp.readyState == 4) 
		{
			if (xhttp.status != 200) 
			{
				console.log("erreur!");
			}
			
			else
			{
				console.log("ok!");
			}
		}
	}
	let corpsRequete = '{"controleAutomatique":' + p_bool + '}';
	xhttp.open("PUT", "/modifControleAuto", true);
	xhttp.send(corpsRequete);
}

function modifierMode()
{
	controleAutomatiqueActive = !controleAutomatiqueActive;
	envoyerModificationMode(controleAutomatiqueActive);
}

function miseAJourMode(p_controleAutomatiqueActive)
{
	let modeManuel = document.getElementsByClassName("modeManuel");
	let modeAuto = document.getElementsByClassName("modeAutomatique");
	let affichageMode = document.getElementById("changerMode");

	if(p_controleAutomatiqueActive)
	{
		// mock test mode = p_controleAutomatiqueActive;
		Array.from(modeManuel).forEach(element => {
			element.classList.add("cacher");
		});

		Array.from(modeAuto).forEach(element => {
			element.classList.remove("cacher");
		});

		affichageMode.textContent = "Activer mode Manuel";
	}
	else
	{
		// mock test mode = p_controleAutomatiqueActive;
		Array.from(modeManuel).forEach(element => {
			element.classList.remove("cacher");
		});

		Array.from(modeAuto).forEach(element => {
			element.classList.add("cacher");
		});
	
		affichageMode.textContent = "Activer mode auto";
	}
}