using ASS_Di_Stroid_API.Data;
using ASS_Di_Stroid_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace ASS_Di_Stroid_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamScoreController : ControllerBase
{
    private readonly DatabaseContext _context;

    public TeamScoreController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost("set-teamscore")]
    public async Task<IActionResult> SetTeamScore([FromQuery] string team_name, [FromQuery] int score)
    {
        if (score <= 0)
        {
            return BadRequest("'score' must be > 0");
        }
        
        TeamScore newTeamScore = new TeamScore()
        {
            team_name = team_name,
            score = score,
            date = DateTime.Now.ToUniversalTime()
        };
        
        _context.TeamScores.Add(newTeamScore);
        await _context.SaveChangesAsync();
        return Ok(newTeamScore);
    }

    [HttpGet("get-teamscore")]
    public async Task<IActionResult> GetTeamScore([FromQuery] string team_name)
    {
        if (team_name == "")
        {
            return BadRequest("Missing 'team_name' parameter");
        }
        
        IQueryable<TeamScore> query = _context.TeamScores.AsQueryable();
        
        query =  query.Where(t => t.team_name == team_name);
        
        TeamScore foundTeam = await query.Take(1).FirstOrDefaultAsync();

        if (foundTeam == null)
        {
            return NotFound($"No team with name {team_name}");
        }

        return Ok(foundTeam);
    }

    [HttpGet("get-next-team")]
    public async Task<IActionResult> GetNextTeam([FromQuery] int current_score)
    {
        if (current_score <= 0)
        {
            return BadRequest("score must be > 0");
        }
        
        List<TeamScore> allScores =  await _context.TeamScores.ToListAsync();
        TeamScore scoreToReturn = null;
        
        allScores.Sort((a, b) => a.score.CompareTo(b.score));

        if (allScores.Last().score < current_score)
        {
            return Ok($"Current Score '{current_score}' is highest");
        }

        foreach (TeamScore teamScore in allScores)
        {
            if (teamScore.score > current_score)
            {
                scoreToReturn = teamScore;
                break;
            }
        }

        if (scoreToReturn == null)
        {
            return Ok($"Current Score '{current_score}' is highest");
        }
        
        return Ok(scoreToReturn);
    }
}